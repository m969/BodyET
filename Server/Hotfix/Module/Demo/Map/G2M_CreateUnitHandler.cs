using System;
using ETModel;
using PF;
using UnityEngine;

namespace ETHotfix
{
	[ActorMessageHandler]
	public class G2M_CreateUnitHandler : AMActorRpcHandler<Scene, G2M_CreateUnit, M2G_CreateUnit>
	{
		protected override async ETTask Run(Scene scene, G2M_CreateUnit request, M2G_CreateUnit response, Action reply)
		{
			Unit unit = EntityFactory.CreateWithId<Unit>(scene, IdGenerater.GenerateId());
			unit.Awake();
			unit.AddComponent<MoveComponent>();
			unit.AddComponent<TransformComponent>();
			unit.AddComponent<UnitPathComponent>();
			unit.Position = new Vector3(-10, 0, -10);

			unit.AddComponent<MailBoxComponent>();
			await unit.AddLocation();
			unit.AddComponent<UnitGateComponent, long>(request.GateSessionId);
			scene.GetComponent<UnitComponent>().Add(unit);
			response.UnitId = unit.Id;
			
			
			// 广播创建的unit
			var createUnits = new M2C_OnEnterView();
			Unit[] units = scene.GetComponent<UnitComponent>().GetAll();
			Log.Debug($"{units.Length} {units}");
			foreach (Unit u in units)
			{
				UnitInfo unitInfo = new UnitInfo();
				unitInfo.X = u.Position.x;
				unitInfo.Y = u.Position.y;
				unitInfo.Z = u.Position.z;
				unitInfo.UnitId = u.Id;
				//createUnits.InViewUnits.Add(unitInfo);
				if (u.Id == unit.Id)
					createUnits.EnterUnit = (unitInfo);
				response.Units.Add(unitInfo);
			}
			//createUnits.SelfUnitId = unit.Id;
			MessageHelper.BroadcastToOther(unit, createUnits);
			//MessageHelper.Send(unit)
			//response.Units.AddRange(createUnits.InViewUnits);
			reply();
		}
	}
}