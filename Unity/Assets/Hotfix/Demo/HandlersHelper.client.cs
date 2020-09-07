using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETModel;
using DG.Tweening;

namespace ETHotfix
{
	public partial class HandlersHelper : HandlersHelperBase
	{
		public override async ETTask UnitOperationHandler(ETModel.Session session, UnitOperation message)
		{
			var unit = UnitComponent.Instance.Get(message.UnitId);
			if (unit == null)
				return;
			if (unit.BodyView == null)
				return;
			var newPosition = new Vector3(message.X / 100f, message.Y / 100f, message.Z / 100f);
			Log.Debug($"UnitOperationHandler newPosition {newPosition}");
			//unit.KinematicCharacterMotor.MoveCharacter(newPosition);
			unit.TransformComponent.SetPosition(newPosition);
			unit.Rotation = new Vector3(0, message.AngleY / 100f, 0);
			//if (message.Operation == OperaType.Fire)
			//{
			//	var x = (int)(message.IntParams[0] / 100f);
			//	var y = (int)(message.IntParams[1] / 100f);
			//	var z = (int)(message.IntParams[2] / 100f);
			//	var bulletType = message.IntParams[3];
			//	var bulletId = message.LongParams[0];
			//	unit.Fire(new Vector3(x, y, z), bulletType, bulletId);
			//}
			await ETTask.CompletedTask;
		}

		public override async ETTask C2M_SetEntityPropertyHandler(ETModel.Session session, C2M_SetEntityProperty message)
		{
		}

		public override async ETTask G2C_TestHotfixMessageHandler(ETModel.Session session, G2C_TestHotfixMessage message)
		{
		}

		public override async ETTask M2C_InViewUnitsHandler(ETModel.Session session, M2C_InViewUnits message)
		{
			CreateLocalUnit(message);
			CreateRemoteUnits(message);
			await ETTask.CompletedTask;
		}

		//创建本地玩家
		private void CreateLocalUnit(M2C_InViewUnits message)
		{
			var localUnit = MongoHelper.FromBson<Unit>(message.SelfUnit.bytes);
			Unit.LocalUnit = localUnit;
			Log.Debug($"localUnit.Position {localUnit.Position} ");
			Log.Debug($"localUnit.Position {localUnit.Components.Count} ");
			//Log.Debug($"localUnit.Position  {localUnit.GetComponent<TransformComponent>()}");
			var go = UnityEngine.Object.Instantiate(PrefabHelper.GetUnitPrefab("LocalUnit"));
			go.transform.position = localUnit.Position;
			GameObject.DontDestroyOnLoad(go);
			foreach (var item in localUnit.Components.Values)
			{
				Log.Debug($"{item.GetType().Name}");
				ETModel.Game.EventSystem.RegisterSystem(item);
				item.Parent = localUnit;
			}
			//var unit = ETModel.EntityFactory.CreateWithId<Unit>(ETModel.Game.Scene, localUnit.Id);
			ETModel.Game.EventSystem.Awake(localUnit);
			localUnit.Awake(go);
			UnitComponent.Instance.Add(localUnit);
			//go.transform.GetChild(2).parent = null;
			//go.transform.GetChild(1).parent = null;
			localUnit.Position = localUnit.Position;
			Game.Scene.AddComponent<OperaComponent>();
		}

		//创建其他玩家
		private void CreateRemoteUnits(M2C_InViewUnits message)
		{
			foreach (var entityJson in message.InViewEntitys)
			{
				OnEnterView(entityJson).Coroutine();
			}
		}

		public override async ETTask M2C_OnEnterViewHandler(ETModel.Session session, M2C_OnEnterView message)
		{
			var entity = await OnEnterView(message.EnterEntity);
			if (entity is Bullet bullet)
			{
				var pos = new Vector3(message.X / 100f, message.Y / 100f, message.Z / 100f);
				bullet.BodyView = GameObject.Instantiate(PrefabHelper.GetUnitPrefab("BulletSmallBlue"), pos, Quaternion.identity);
				bullet.BodyView.name = $"Bullet#{bullet.Id}";
				bullet.TransformComponent.transform = bullet.BodyView.transform;
				bullet.TransformComponent.SetPosition(pos);
				Log.Debug($"bullet {pos}");
			}
			await ETTask.CompletedTask;
		}

		public static async ETTask<ETModel.Entity> OnEnterView(EntiyInfo entityInfo)
		{
			try
			{
				Log.Debug($"{EntityDefine.GetType(entityInfo.Type).Name}");
				if (entityInfo.Type == EntityDefine.GetTypeId<Unit>())
				{
					var remoteUnit = MongoHelper.FromBson<Unit>(entityInfo.BsonBytes.bytes);
					foreach (var item in remoteUnit.Components)
						Log.Debug($"remoteUnit {item.GetType().Name}");
					//remoteUnit.Domain = ETModel.Game.Scene;
					//Unit unit = UnitFactory.Create(ETModel.Game.Scene, remoteUnit.Id);
					var go = UnityEngine.Object.Instantiate(PrefabHelper.GetUnitPrefab("RemoteUnit"));
					go.transform.position = remoteUnit.Position;
					GameObject.DontDestroyOnLoad(go);
					//var unit = EntityFactory.CreateWithId<Unit>(domain, id);
					ETModel.Game.EventSystem.RegisterSystem(remoteUnit);
					ETModel.Game.EventSystem.Awake(remoteUnit);
					remoteUnit.Awake(go);
					UnitComponent.Instance.Add(remoteUnit);
					remoteUnit.Position = remoteUnit.Position;
					//remoteUnit.Dispose();
					return remoteUnit;
				}
				if (entityInfo.Type == EntityDefine.GetTypeId<Bullet>())
				{
					var remoteBullet = MongoHelper.FromBson<Bullet>(entityInfo.BsonBytes.bytes);
					//Log.Debug($"{remoteBullet}");
					//var bullet = ETModel.EntityFactory.CreateWithId<Bullet>(ETModel.Game.Scene, remoteBullet.Id);
					Log.Debug($"{remoteBullet.Components.Count}");
					ETModel.Game.EventSystem.RegisterSystem(remoteBullet);
					ETModel.Game.EventSystem.Awake(remoteBullet);
					BulletComponent.Instance.Add(remoteBullet);
					//remoteBullet.Dispose();
					return remoteBullet;
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
		
		public override async ETTask M2C_OnEntityChangedHandler(ETModel.Session session, M2C_OnEntityChanged message)
		{
			Log.Debug($"M2C_OnEntityChangedHandler {Dumper.DumpAsString(message)}");
			var entityType = EntityDefine.TypeIds.GetKeyByValue((ushort)message.EntityType);
			ETModel.Entity entity = null;
			if (entityType == typeof(Unit))
			{
				entity = UnitComponent.Instance.Get(message.EntityId);
			}
			if (entityType == typeof(Bullet))
			{
				var bullet = BulletComponent.Instance.Get(message.EntityId);
				entity = bullet;
				if (entity is null)
					return;
				if (bullet.BodyView != null)
					bullet.BodyView.transform.DOMove(new Vector3(message.X / 100f, message.Y / 100f, message.Z / 100f), 0.2f);
			}
			if (entityType == typeof(Monster))
			{
				var monster = MonsterComponent.Instance.Get(message.EntityId);
				entity = monster;
				if (entity is null)
					return;
				if (monster.BodyView != null)
					monster.BodyView.transform.DOMove(new Vector3(message.X / 100f, message.Y / 100f, message.Z / 100f), 0.2f);
			}
			if (entity is null)
				return;
			if (message.ComponentType > 0)
				entity = entity.GetComponent(EntityDefine.GetType(message.ComponentType));

			var propertyCollection = EntityDefine.PropertyCollectionMap[EntityDefine.GetTypeId(entity.GetType())];
			if (propertyCollection.ContainsKey((ushort)message.PropertyId))
				entity.SetPropertyValue((ushort)message.PropertyId, message.PropertyValue.bytes);
			await ETTask.CompletedTask;
		}

		public override async ETTask M2C_OnLeaveViewHandler(ETModel.Session session, M2C_OnLeaveView message)
		{
			if (message.EntityType == EntityDefine.GetTypeId<Unit>())
			{
				UnitComponent.Instance.Remove(message.LeaveEntity);
			}
			if (message.EntityType == EntityDefine.GetTypeId<Bullet>())
			{
				var bullet = BulletComponent.Instance.Get(message.LeaveEntity);
				if (bullet != null)
				{
					BulletComponent.Instance.Remove(message.LeaveEntity);
				}
			}
			await ETTask.CompletedTask;
		}

		public override async ETTask PlayerInfoHandler(ETModel.Session session, PlayerInfo message)
		{
		}
	}
}