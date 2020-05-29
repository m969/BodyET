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
			try
			{
				Log.Debug($"{entityInfo.Type}");
				if (entityInfo.Type == EntityDefine.GetTypeId<Unit>())
				{
					var remoteUnit = MongoHelper.FromBson<Unit>(entityInfo.BsonBytes.bytes);
					//Log.Debug($"{remoteUnit}");
					Unit unit = UnitFactory.Create(ETModel.Game.Scene, remoteUnit.Id);
					unit.Position = remoteUnit.Position;
					remoteUnit.Dispose();
					return unit;
				}
				if (entityInfo.Type == EntityDefine.GetTypeId<Bullet>())
				{
					var remoteBullet = MongoHelper.FromBson<Bullet>(entityInfo.BsonBytes.bytes);
					//Log.Debug($"{remoteBullet}");
					var bullet = ETModel.EntityFactory.CreateWithId<Bullet>(ETModel.Game.Scene, remoteBullet.Id);
					BulletComponent.Instance.Add(bullet);
					remoteBullet.Dispose();
					return bullet;
				}
				if (entityInfo.Type == EntityDefine.GetTypeId<Monster>())
				{
					var remote = MongoHelper.FromBson<Monster>(entityInfo.BsonBytes.bytes);
					Log.Debug($"HealthComponent HP{remote.GetComponent<HealthComponent>().HP}");
					remote.Awake();
					remote.Domain = ETModel.Game.Scene;
					remote.BodyView = GameObject.Instantiate(PrefabHelper.GetUnitPrefab("Monster"));
					GameObject.DontDestroyOnLoad(remote.BodyView);
					//var monster = MonsterFactory.Create(ETModel.Game.Scene, remote.Id);
					MonsterComponent.Instance.Add(remote);
					//remote.Position = remote.Position;
					//remote.Dispose();
					return remote;
				}
			}
			catch (System.Exception e)
			{
				Log.Error(e);
			}
			return null;
		}
    }
}
