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
		public override async ETTask C2G_LoginGateHandler(Session session, C2G_LoginGate request, G2C_LoginGate response, Action reply)
		{
			Console.WriteLine("C2G_LoginGateHandler");
			Scene scene = Game.Scene.Get(request.GateId);
			if (scene == null)
			{
				return;
			}

			string account = scene.GetComponent<GateSessionKeyComponent>().Get(request.Key);
			if (account == null)
			{
				response.Error = ErrorCode.ERR_ConnectGateKeyError;
				response.Message = "Gate key验证失败!";
				reply();
				return;
			}

			var results = await DBComponent.Instance.Query<Player>(x => x.Account == account);
			Player player = null;
			if (results.Count > 0)
			{
				player = results[0];
			}
			else
			{
				player = EntityFactory.Create<Player, string>(Game.Scene, account);
				DBComponent.Instance.Save(player).Coroutine();
			}
			scene.GetComponent<PlayerComponent>().Add(player);
			session.AddComponent<SessionPlayerComponent>().Player = player;
			session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);

			response.PlayerId = player.Id;
			reply();

			//session.Send(new G2C_TestHotfixMessage() { Info = "recv hotfix message success" });
			await ETTask.CompletedTask;
		}

		public override async ETTask C2G_EnterMapHandler(Session session, C2G_EnterMap request, G2C_EnterMap response, Action reply)
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
		
		public override async ETTask R2G_GetLoginKeyHandler(Scene scene, R2G_GetLoginKey request, G2R_GetLoginKey response, Action reply)
		{
			long key = RandomHelper.RandInt64();
			scene.GetComponent<GateSessionKeyComponent>().Add(key, request.Account);
			response.Key = key;
			response.GateId = scene.Id;
			reply();
			await ETTask.CompletedTask;
		}
	}
}