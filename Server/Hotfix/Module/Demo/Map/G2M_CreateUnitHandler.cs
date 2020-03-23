using System;
using ETModel;
using PF;
using UnityEngine;
using System.Linq;

namespace ETHotfix
{
	[ActorMessageHandler]
	public class G2M_CreateUnitHandler : AMActorRpcHandler<Scene, G2M_CreateUnit, M2G_CreateUnit>
	{
		protected override async ETTask Run(Scene scene, G2M_CreateUnit request, M2G_CreateUnit response, Action reply)
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

			Unit unit = EntityFactory.CreateWithId<Unit>(copyMap, IdGenerater.GenerateId());
			unit.Initialize();

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
				//UnitInfo unitInfo = new UnitInfo();
				//unitInfo.X = u.Position.x;
				//unitInfo.Y = u.Position.y;
				//unitInfo.Z = u.Position.z;
				//unitInfo.UnitId = u.Id;
				var entityInfo = new EntiyInfo();
				entityInfo.BsonBytes = new Google.Protobuf.ByteString();
				entityInfo.BsonBytes.bytes = MongoHelper.ToBson(u);
				entityInfo.Type = EntityDefine.GetTypeId(u.GetType());
				if (u.Id == unit.Id)
				{
					enterViewMsg.EnterEntity = entityInfo;
					inViewUnitsMsg.SelfUnit = entityInfo.BsonBytes;
					continue;
				}
				inViewUnitsMsg.InViewEntitys.Add(entityInfo);
				//response.Units.Add(unitInfo);
			}
			MessageHelper.BroadcastToOther(unit, enterViewMsg);
			MessageHelper.Send(unit, inViewUnitsMsg);
			//response.Units.AddRange(createUnits.InViewUnits);
			reply();
		}
	}
}