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
        public override async ETTask C2R_LoginHandler(Session session, C2R_Login request, R2C_Login response, Action reply)
        {
            Console.WriteLine("C2R_LoginHandler");
            // 随机分配一个Gate
            StartConfig config = RealmGateAddressHelper.GetGate();
            //Log.Debug($"gate address: {MongoHelper.ToJson(config)}");

            // 向gate请求一个key,客户端可以拿着这个key连接gate
            G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey)await ActorMessageSenderComponent.Instance.Call(
                config.SceneInstanceId, new R2G_GetLoginKey() { Account = request.Account });

            string outerAddress = config.GetParent<StartConfig>().GetComponent<OuterConfig>().Address2;

            response.Address = outerAddress;
            response.Key = g2RGetLoginKey.Key;
            response.GateId = g2RGetLoginKey.GateId;
            reply();
        }

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

        public override async ETTask C2M_TestActorRequestHandler(Unit unit, C2M_TestActorRequest request, M2C_TestActorResponse response, Action reply)
        {
            response.Info = "actor rpc response";
            reply();
			await ETTask.CompletedTask;
        }

        public override async ETTask C2G_PlayerInfoHandler(Session session, C2G_PlayerInfo request, G2C_PlayerInfo response, Action reply)
        {
			response.PlayerInfo = new PlayerInfo();
			response.PlayerInfos.Add(new PlayerInfo() { RpcId = 1 });
			response.PlayerInfos.Add(new PlayerInfo() { RpcId = 2 });
			response.PlayerInfos.Add(new PlayerInfo() { RpcId = 3 });
			response.TestRepeatedInt32.Add(4);
			response.TestRepeatedInt32.Add(5);
			response.TestRepeatedInt32.Add(6);
			response.TestRepeatedInt64.Add(7);
			response.TestRepeatedInt64.Add(8);
			response.TestRepeatedString.Add("9");
			response.TestRepeatedString.Add("10");
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
    }
}