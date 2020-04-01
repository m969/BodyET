using System;
using ETModel;

namespace ETHotfix
{
	[MessageHandler]
	public class C2G_EnterMapHandler : AMRpcHandler<C2G_EnterMap, G2C_EnterMap>
	{
		protected override async ETTask Run(Session session, C2G_EnterMap request, G2C_EnterMap response, Action reply)
		{
			Console.WriteLine("C2G_EnterMapHandler");
			Player player = session.GetComponent<SessionPlayerComponent>().Player;

			var createUnitRequest = new G2M_CreateUnit() { PlayerId = player.Id, GateSessionId = session.InstanceId };
			createUnitRequest.UnitId = player.UnitId;

			// 在map服务器上创建战斗Unit
			long mapInstanceId = StartConfigComponent.Instance.GetByName("Map1").SceneInstanceId;
			var createUnit = (M2G_CreateUnit)await ActorMessageSenderComponent.Instance.Call(mapInstanceId, createUnitRequest);
			player.UnitId = createUnit.UnitId;
			DBComponent.Instance.Save(player).Coroutine();
			
			response.UnitId = createUnit.UnitId;
			reply();
		}
	}
}