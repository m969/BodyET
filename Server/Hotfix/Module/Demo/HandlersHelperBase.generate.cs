using System;
using ETModel;
namespace ETHotfix
{
	public class HandlersHelperBase
	{
		 public virtual async ETTask C2R_LoginHandler(Scene scene, C2R_Login request, R2C_Login response, Action reply){}
		 public virtual async ETTask C2G_LoginGateHandler(Scene scene, C2G_LoginGate request, G2C_LoginGate response, Action reply){}
		 public virtual async ETTask G2C_TestHotfixMessageHandler(Scene scene, G2C_TestHotfixMessage request, MessageResponse response, Action reply){}
		 public virtual async ETTask C2M_TestActorRequestHandler(Scene scene, C2M_TestActorRequest request, MessageResponse response, Action reply){}
		 public virtual async ETTask PlayerInfoHandler(Scene scene, PlayerInfo request, MessageResponse response, Action reply){}
		 public virtual async ETTask C2G_PlayerInfoHandler(Scene scene, C2G_PlayerInfo request, G2C_PlayerInfo response, Action reply){}
		 public virtual async ETTask UnitOperationHandler(Scene scene, UnitOperation request, MessageResponse response, Action reply){}
		 public virtual async ETTask M2C_OnEntityChangedHandler(Scene scene, M2C_OnEntityChanged request, MessageResponse response, Action reply){}
		 public virtual async ETTask C2M_SetEntityPropertyHandler(Scene scene, C2M_SetEntityProperty request, MessageResponse response, Action reply){}
		 public virtual async ETTask FireRequestHandler(Scene scene, FireRequest request, MessageResponse response, Action reply){}
		 public virtual async ETTask G2M_GetCopyMapHandler(Scene scene, G2M_GetCopyMap request, MessageResponse response, Action reply){}
	}
}
