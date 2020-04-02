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
			//self.AddComponent<TransformComponent>().Position = position;
			self.Position = position;
			self.AddComponent<MoveComponent>().Speed = 30;
			self.AddComponent<Body2dComponent>().CreateBody(.2f, .2f);
			self.GetComponent<Body2dComponent>().OnBeginContactAction += self.OnBeginContact;
		}

		public static void MoveTo(this Bullet self, Vector3 position)
		{
			self.GetComponent<MoveComponent>().Target = self.Transform.Position;
			self.GetComponent<MoveComponent>().MoveTo(position).Coroutine();
		}

		public static void OnBeginContact(this Bullet self, Body2dComponent other)
		{
			if (other.Parent is Unit unit)
			{
				if (self.OwnerId != unit.Id)
				{
					unit.HP -= 10;
					if (unit.HP <= 0)
					{
						unit.Dead();
					}
					self.Dispose();
				}
			}
		}

		public static void Destroy(this Bullet self)
		{
			MessageHelper.Broadcast(self.Domain, new M2C_OnLeaveView() { LeaveEntity = self.Id, EntityType = EntityDefine.EntityIds.GetValueByKey(typeof(Bullet)) });
		}

		public static void Update(this Bullet self)
		{
			if (TimeHelper.Now() - self.Timer > self.Internal)
			{
				self.Timer = TimeHelper.Now();
				var lp = self.Transform.LastPosition;
				var p = self.Transform.Position;
				if (Vector3.Distance(lp, p) < 0.1f)
					return;
				self.Transform.LastPosition = p;

				var msg = new M2C_OnEntityChanged();
				msg.EntityId = self.Id;
				msg.EntityType = EntityDefine.GetTypeId<Bullet>();

				msg.X = (int)(p.x * 100);
				msg.Y = (int)(p.y * 100);
				msg.Z = (int)(p.z * 100);
				MessageHelper.Broadcast(self.Domain, msg);
			}
		}
	}
}