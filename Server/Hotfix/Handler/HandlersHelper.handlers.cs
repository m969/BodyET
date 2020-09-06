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
		public override async ETTask C2G_PlayerInfoHandler(Session session, C2G_PlayerInfo request, G2C_PlayerInfo response, Action reply)
		{
			//response.PlayerInfo = new PlayerInfo();
			//response.PlayerInfos.Add(new PlayerInfo() { RpcId = 1 });
			//response.PlayerInfos.Add(new PlayerInfo() { RpcId = 2 });
			//response.PlayerInfos.Add(new PlayerInfo() { RpcId = 3 });
			//response.TestRepeatedInt32.Add(4);
			//response.TestRepeatedInt32.Add(5);
			//response.TestRepeatedInt32.Add(6);
			//response.TestRepeatedInt64.Add(7);
			//response.TestRepeatedInt64.Add(8);
			//response.TestRepeatedString.Add("9");
			//response.TestRepeatedString.Add("10");
			reply();
			await ETTask.CompletedTask;
		}

		public override async ETTask C2M_ReloadHandler(Session session, C2M_Reload request, M2C_Reload response, Action reply)
		{
			Log.Debug($"C2M_ReloadHandler {MongoHelper.ToJson(request)}");
			if (request.Account != "panda" && request.Password != "panda")
			{
				Log.Error($"error reload account and password: {MongoHelper.ToJson(request)}");
				return;
			}
			StartConfigComponent startConfigComponent = Game.Scene.GetComponent<StartConfigComponent>();
			NetInnerComponent netInnerComponent = Game.Scene.GetComponent<NetInnerComponent>();
			foreach (StartConfig startConfig in startConfigComponent.GetAll())
			{
				InnerConfig innerConfig = startConfig.GetComponent<InnerConfig>();
				Session serverSession = netInnerComponent.Get(innerConfig.Address);
				await serverSession.Call(new M2A_Reload());
			}
			reply();
		}

		public override async ETTask C2R_PingHandler(Session session, C2R_Ping request, R2C_Ping response, Action reply)
		{
			reply();
			await ETTask.CompletedTask;
		}

		public override async ETTask M2A_ReloadHandler(Scene scene, M2A_Reload request, A2M_Reload response, Action reply)
		{
			Game.EventSystem.Add(DLLType.Hotfix, DllHelper.GetHotfixAssembly());
			reply();
			await ETTask.CompletedTask;
		}

		public static int count = 0;
		public override async ETTask G2C_TestHandler(Session session, G2C_Test request)
		{
			count++;
			await ETTask.CompletedTask;
		}
	}
}