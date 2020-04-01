using ETModel;
using PF;
using Vector3 = UnityEngine.Vector3;
using UnityEngine;
using System.Collections.Generic;

namespace ETHotfix
{
	[MessageHandler]
	public class M2C_OnEnterViewHandler : AMHandler<M2C_OnEnterView>
	{
		protected override async ETTask Run(ETModel.Session session, M2C_OnEnterView message)
		{
			var entity = await OnEnterView(message.EnterEntity);
			if (entity is Bullet bullet)
			{
				bullet.BodyView = GameObject.Instantiate(PrefabHelper.GetUnitPrefab("BulletSmallBlue"));
				bullet.BodyView.name = $"Bullet#{bullet.Id}";
				bullet.Position = new Vector3(message.X / 100f, message.Y / 100f, message.Z / 100f);
			}
			await ETTask.CompletedTask;
		}
		
		public static async ETTask<ETModel.Entity> OnEnterView(EntiyInfo entityInfo)
		{
			if (entityInfo.Type == EntityDefine.GetTypeId<Unit>())
			{
				var remoteUnit = MongoHelper.FromBson<Unit>(entityInfo.BsonBytes.bytes);
				//if (remoteUnit.PosArr != null)
				//	Log.Msg(remoteUnit.PosArr);
				Unit unit = UnitFactory.Create(ETModel.Game.Scene, remoteUnit.Id);
				unit.Position = remoteUnit.Position;
				//unit.Position = new Vector3(remoteUnit.PosArr[0], remoteUnit.PosArr[1], remoteUnit.PosArr[2]);
				remoteUnit.Dispose();
				return unit;
			}
			if (entityInfo.Type == EntityDefine.GetTypeId<Bullet>())
			{
				var remoteBullet = MongoHelper.FromBson<Bullet>(entityInfo.BsonBytes.bytes);
				//if (remoteBullet.PosArr != null)
				//	Log.Msg(remoteBullet.PosArr);
				var bullet = ETModel.EntityFactory.CreateWithId<Bullet>(ETModel.Game.Scene, remoteBullet.Id);
				BulletComponent.Instance.Add(bullet);
				remoteBullet.Dispose();
				return bullet;
			}
			return null;
		}
    }
}
