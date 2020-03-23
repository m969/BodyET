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

	public static class BulletSystem
	{
		public static void Destroy(this Bullet self)
		{
			MessageHelper.Broadcast(self.Domain, new M2C_OnLeaveView() { LeaveEntity = self.Id, EntityType = EntityDefine.EntityIds.GetValueByKey(typeof(Bullet)) });
		}

		public static void Update(this Bullet self)
		{
			if (TimeHelper.Now() - self.Timer > self.Internal)
			{
				self.Timer = TimeHelper.Now();
				var lp = self.GetComponent<TransformComponent>().LastPosition;
				var p = self.GetComponent<TransformComponent>().Position;
				if (Vector3.Distance(lp, p) < 0.1f)
					return;
				self.GetComponent<TransformComponent>().LastPosition = p;

				var msg = new M2C_OnEntityChanged();
				msg.EntityId = self.Id;
				msg.EntityType = EntityDefine.GetTypeId(typeof(Bullet));

				msg.X = (int)(p.x * 100);
				msg.Y = (int)(p.y * 100);
				msg.Z = (int)(p.z * 100);
				MessageHelper.Broadcast(self.Domain, msg);
			}
		}
	}
}