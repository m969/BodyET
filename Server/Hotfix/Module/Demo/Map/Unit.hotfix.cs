using System.Collections.Generic;
using System.Threading;
using ETModel;
using PF;
using UnityEngine;

namespace ETHotfix
{
	[ObjectSystem]
	public class UnitUpdateSystem : UpdateSystem<Unit>
	{
		public override void Update(Unit self)
		{
			self.Update();
		}
	}

	[ObjectSystem]
	public class UnitDestroySystem : DestroySystem<Unit>
	{
		public override void Destroy(Unit self)
		{
			self.Destroy();
		}
	}

	public static class UnitHotfix
    {
       public static Bullet Fire(this Unit self, UnitOperation message)
        {
			var bullet = EntityFactory.Create<Bullet>(self.Domain);
			bullet.OwnerId = self.Id;
			bullet.Setup(self.Position);

			var msg = new M2C_OnEnterView();
			msg.EnterEntity = new EntiyInfo();
			msg.EnterEntity.Type = EntityDefine.GetTypeId<Bullet>();
			msg.EnterEntity.BsonBytes.bytes = MongoHelper.ToBson(bullet);
			var p = bullet.Position;
			msg.X = (int)(p.x * 100);
			msg.Y = (int)(p.y * 100);
			msg.Z = (int)(p.z * 100);
			MessageHelper.Broadcast(self.Domain, msg);

			var targetPoint = new Vector3();
			targetPoint.x = message.IntParams[0] / 100f;
			targetPoint.y = message.IntParams[1] / 100f;
			targetPoint.z = message.IntParams[2] / 100f;
			bullet.MoveTo(targetPoint);
			return bullet;
		}

		public static void Update(this Unit self)
		{
			if (self.State == 2)
			{
				if (self.Position != self.LastPosition)
				{
					self.IdleTimer = TimeHelper.Now();
					self.LastPosition = self.Position;
				}
				if (TimeHelper.Now() - self.IdleTimer > 200)
				{
					self.State = 1;
				}
			}
		}

		public static void Destroy(this Unit self)
		{
			MessageHelper.Broadcast(self.Domain, new M2C_OnLeaveView() { LeaveEntity = self.Id, EntityType = EntityDefine.GetTypeId<Unit>() });
		}

		public static async ETTask Save(this Unit self)
		{
			await DBComponent.Instance.Save(self);
		}
	}
}