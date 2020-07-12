using System.Collections.Generic;
using System.Threading;
using ETModel;
using PF;
using UnityEngine;

namespace ETHotfix
{
	[ObjectSystem]
	public class BulletAwakeSystem : AwakeSystem<Bullet>
	{
		public override void Awake(Bullet self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class BulletUpdateSystem : UpdateSystem<Bullet>
	{
		public override void Update(Bullet self)
		{
			self.Update();
		}
	}

	[ObjectSystem]
	public class BulletDestroySystem : DestroySystem<Bullet>
	{
		public override void Destroy(Bullet self)
		{
			self.Destroy();
		}
	}

	public static class BulletHotfix
	{
		public static void Setup(this Bullet self, Vector3 position)
		{
			//self.Position = position;
			self.AddComponent<TransformComponent>().position = position;
			self.AddComponent<MoveComponent>().Speed = 30;
			self.AddComponent<EntitySyncComponent>();
			self.AddComponent<Body2dComponent>().CreateBody(.2f, .2f);
			self.GetComponent<Body2dComponent>().OnBeginContactAction += self.OnBeginContact;
		}

		public static void MoveTo(this Bullet self, Vector3 position)
		{
			Log.Debug($"Bullet MoveTo {position}");
			self.GetComponent<MoveComponent>().Target = self.Position;
			self.GetComponent<MoveComponent>().MoveTo(position, true).Coroutine();
		}

		public static void OnBeginContact(this Bullet self, Body2dComponent other)
		{
			if (other.Parent is Unit unit)
			{
				if (self.OwnerId != unit.Id)
				{
					if (unit.State == 0 || self.IsDisposed)
					{
						return;
					}
					unit.GetComponent<HealthComponent>().DoDamage(10);
					self.Dispose();
				}
			}
			if (other.Parent is Monster monster)
			{
				if (self.OwnerId != monster.Id || self.IsDisposed)
				{
					//if (monster.State == 0)
					//{
					//	return;
					//}
					monster.GetComponent<HealthComponent>().DoDamage(10);
					//if (monster.GetComponent<HealthComponent>().HP == 0)
					//	MonsterComponent.Instance.Remove(monster.Id);
					self.Dispose();
				}
			}
		}

		public static void Destroy(this Bullet self)
		{
			MessageHelper.Broadcast(self.Domain, new M2C_OnLeaveView() { LeaveEntity = self.Id, EntityType = EntityDefine.GetTypeId<Bullet>() });
		}

		public static void Update(this Bullet self)
		{
			//Log.Debug($"BulletHotfix Position={self.Position}");

			//if (TimeHelper.Now() - self.Timer > self.Interval)
			//{
			//	self.Timer = TimeHelper.Now();
			//	var lp = self.Transform.LastPosition;
			//	var p = self.Transform.Position;
			//	if (Vector3.Distance(lp, p) < 0.1f)
			//		return;
			//	self.Transform.LastPosition = p;

			//	var msg = new M2C_OnEntityChanged();
			//	msg.EntityId = self.Id;
			//	msg.EntityType = EntityDefine.GetTypeId<Bullet>();

			//	msg.X = (int)(p.x * 100);
			//	msg.Y = (int)(p.y * 100);
			//	msg.Z = (int)(p.z * 100);
			//	MessageHelper.Broadcast(self.Domain, msg);
			//}
		}
	}
}