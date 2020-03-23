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

	public static class UnitSystem
    {
       public static async ETTask<Bullet> Fire(this Unit self, UnitOperation message)
        {
			var bullet = EntityFactory.Create<Bullet>(self.Domain);
			bullet.OwnerId = self.Id;
			bullet.AddComponent<TransformComponent>();
			bullet.Transform.Position = self.Position;
			var bulletMove = bullet.AddComponent<MoveComponent>();
			bulletMove.Speed = 30;
			bullet.AddComponent<Body2dComponent>().CreateBody(.2f, .2f);

			var msg = new M2C_OnEnterView();
			msg.EnterEntity = new EntiyInfo();
			msg.EnterEntity.Type = EntityDefine.GetTypeId(bullet.GetType());
			msg.EnterEntity.BsonBytes.bytes = MongoHelper.ToBson(bullet);
			var p = bullet.Transform.Position;
			msg.X = (int)(p.x * 100);
			msg.Y = (int)(p.y * 100);
			msg.Z = (int)(p.z * 100);
			MessageHelper.Broadcast(self.Domain, msg);

			bulletMove.Target = bullet.Transform.Position;
			var targetPoint = new Vector3();
			targetPoint.x = message.IntParams[0] / 100f;
			targetPoint.y = message.IntParams[1] / 100f;
			targetPoint.z = message.IntParams[2] / 100f;
			bulletMove.MoveTo(targetPoint, true).Coroutine();
			//var token = new ETCancellationTokenSource();
			//bulletMove.MoveToAsync(targetPoint, token.Token).Coroutine();

			//await TimerComponent.Instance.WaitAsync(2000);
			//token.Cancel();
			//bullet.Dispose();
			return bullet;
		}

		public static void Destroy(this Unit self)
		{
			MessageHelper.Broadcast(self.Domain, new M2C_OnLeaveView() { LeaveEntity = self.Id, EntityType = EntityDefine.EntityIds.GetValueByKey(typeof(Unit)) });
		}
	}
}