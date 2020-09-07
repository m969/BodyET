using System;
using ETModel;
using PF;
using UnityEngine;
using System.Linq;
using ETHotfix;

namespace ETHotfix
{
	public partial class HandlersHelper: HandlersHelperBase
	{
        public override async ETTask C2M_TestActorRequestHandler(Unit unit, C2M_TestActorRequest request, M2C_TestActorResponse response, Action reply)
        {
            response.Info = "actor rpc response";
            reply();
			await ETTask.CompletedTask;
        }

        public override async ETTask UnitOperationHandler(Unit unit, UnitOperation message)
        {
			Log.Msg(message);
			//Log.Debug($"UnitOperationHandler {message}");
			unit.Position = new Vector3(message.X / 100f, message.Y / 100f, message.Z / 100f);
			if (message.Operation == OperaType.Fire)
			{
				unit.Fire(message);
			}
			MessageHelper.BroadcastToOther(unit, message);
			await ETTask.CompletedTask;
        }

        public override async ETTask C2M_SetEntityPropertyHandler(Unit unit, C2M_SetEntityProperty message)
        {
			Log.Msg(message);
			var propertyCollection = EntityDefine.PropertyCollectionMap[EntityDefine.GetTypeId<Unit>()];
			if (propertyCollection.ContainsKey((ushort)message.PropertyId))
				unit.SetPropertyValue((ushort)message.PropertyId, message.PropertyValue.bytes);
			await ETTask.CompletedTask;
        }

		public override async ETTask FireRequestHandler(Unit unit, FireRequest request, MessageResponse response, Action reply)
		{
			MessageHelper.Broadcast(unit, request);
			reply();
			await ETTask.CompletedTask;
		}

		public override async ETTask G2M_CreateUnitHandler(Scene scene, G2M_CreateUnit request, M2G_CreateUnit response, Action reply)
		{
			var copyMap = Game.Scene.Children.Values.ToList().Find((x) =>
			{
				if (x is Scene s)
					return s.Name == "CopyMap1";
				return false;
			});
			if (copyMap == null)
			{
				var copyMapConfig = StartConfigComponent.Instance.GetByName("CopyMap1");
				copyMap = await SceneFactory.Create(Game.Scene, copyMapConfig.GetComponent<SceneConfig>().Name, SceneType.Map);
			}

			Unit unit = null;
			if (request.UnitId != 0)
			{
				unit = await DBComponent.Instance.Query<Unit>(request.UnitId);
				unit.Domain = copyMap;
			}
			else
			{
				unit = EntityFactory.CreateWithId<Unit>(copyMap, IdGenerater.GenerateId());
				unit.PlayerId = request.PlayerId;
				unit.Setup();
				unit.Save().Coroutine();
			}

			unit.AddComponent<MoveComponent>();
			unit.AddComponent<Body2dComponent>().CreateBody(.2f, .2f);
			unit.AddComponent<MailBoxComponent>();
			await unit.AddLocation();
			unit.AddComponent<UnitGateComponent, long>(request.GateSessionId);
			copyMap.GetComponent<UnitComponent>().Add(unit);
			response.UnitId = unit.Id;

			// 广播创建的unit
			var inViewUnitsMsg = new M2C_InViewUnits();
			var enterViewMsg = new M2C_OnEnterView();
			Unit[] units = copyMap.GetComponent<UnitComponent>().GetAll();
			foreach (Unit u in units)
			{
				var entityInfo = new EntiyInfo();
				entityInfo.BsonBytes = new Google.Protobuf.ByteString();
				entityInfo.BsonBytes.bytes = MongoHelper.ToBson(u);
				entityInfo.Type = EntityDefine.GetTypeId<Unit>();
				if (u.Id == unit.Id)
				{
					enterViewMsg.EnterEntity = entityInfo;
					inViewUnitsMsg.SelfUnit = entityInfo.BsonBytes;
					continue;
				}
				inViewUnitsMsg.InViewEntitys.Add(entityInfo);
			}
			var monsters = copyMap.GetComponent<MonsterComponent>().GetAll();
			foreach (var u in monsters)
			{
				var entityInfo = new EntiyInfo();
				entityInfo.BsonBytes = new Google.Protobuf.ByteString();
				entityInfo.BsonBytes.bytes = MongoHelper.ToBson(u);
				entityInfo.Type = EntityDefine.GetTypeId<Monster>();
				inViewUnitsMsg.InViewEntitys.Add(entityInfo);
			}
			MessageHelper.BroadcastToOther(unit, enterViewMsg);
			MessageHelper.Send(unit, inViewUnitsMsg);
			reply();
		}

		public override async ETTask G2M_SessionDisconnectHandler(Unit unit, G2M_SessionDisconnect message)
		{
			unit.GetComponent<UnitGateComponent>().IsDisconnect = true;
			await unit.Save();
			PlayerComponent.Instance.Remove(unit.PlayerId);
			unit.Domain.GetComponent<UnitComponent>().Remove(unit.Id);
			await ETTask.CompletedTask;
		}
	}
}