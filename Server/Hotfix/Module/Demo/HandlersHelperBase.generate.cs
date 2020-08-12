using System;
using ETModel;
namespace ETHotfix
{
	public class HandlersHelperBase
	{
		public static HandlersHelperBase Instance { get; set; }
		public virtual async ETTask C2R_LoginHandler(Session session, C2R_Login request, R2C_Login response, Action reply){}
		public virtual async ETTask C2G_LoginGateHandler(Session session, C2G_LoginGate request, G2C_LoginGate response, Action reply){}
		public virtual async ETTask G2C_TestHotfixMessageHandler(Session session, G2C_TestHotfixMessage request){}
		public virtual async ETTask C2M_TestActorRequestHandler(Unit unit, C2M_TestActorRequest request, M2C_TestActorResponse response, Action reply){}
		public virtual async ETTask PlayerInfoHandler(Session session, PlayerInfo request){}
		public virtual async ETTask C2G_PlayerInfoHandler(Session session, C2G_PlayerInfo request, G2C_PlayerInfo response, Action reply){}
		public virtual async ETTask UnitOperationHandler(Unit unit, UnitOperation message){}
		public virtual async ETTask M2C_InViewUnitsHandler(Scene scene, M2C_InViewUnits message){}
		public virtual async ETTask M2C_OnEnterViewHandler(Scene scene, M2C_OnEnterView message){}
		public virtual async ETTask M2C_OnLeaveViewHandler(Scene scene, M2C_OnLeaveView message){}
		public virtual async ETTask M2C_OnEntityChangedHandler(Unit unit, M2C_OnEntityChanged message){}
		public virtual async ETTask C2M_SetEntityPropertyHandler(Unit unit, C2M_SetEntityProperty message){}
		public virtual async ETTask FireRequestHandler(Unit unit, FireRequest request, MessageResponse response, Action reply){}
		public virtual async ETTask G2M_GetCopyMapHandler(Session session, G2M_GetCopyMap request, MessageResponse response, Action reply){}
	}
	[MessageHandler]
	public class C2R_LoginHandler : AMRpcHandler<C2R_Login, R2C_Login>
	{
		 protected override async ETTask Run(Session session, C2R_Login request, R2C_Login response, Action reply){
		await HandlersHelperBase.Instance.C2R_LoginHandler(session, request, response, reply);
	}
	}
	[MessageHandler]
	public class C2G_LoginGateHandler : AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
	{
		 protected override async ETTask Run(Session session, C2G_LoginGate request, G2C_LoginGate response, Action reply){
		await HandlersHelperBase.Instance.C2G_LoginGateHandler(session, request, response, reply);
	}
	}
	[MessageHandler]
	public class G2C_TestHotfixMessageHandler : AMHandler<G2C_TestHotfixMessage>
	{
		 protected override async ETTask Run(Session session, G2C_TestHotfixMessage request){
		await HandlersHelperBase.Instance.G2C_TestHotfixMessageHandler(session, request);
	}
	}
	[ActorMessageHandler]
	public class C2M_TestActorRequestHandler : AMActorLocationRpcHandler<Unit, C2M_TestActorRequest, M2C_TestActorResponse>
	{
		 protected override async ETTask Run(Unit unit, C2M_TestActorRequest request, M2C_TestActorResponse response, Action reply){
		await HandlersHelperBase.Instance.C2M_TestActorRequestHandler(unit, request, response, reply);
	}
	}
	[MessageHandler]
	public class PlayerInfoHandler : AMHandler<PlayerInfo>
	{
		 protected override async ETTask Run(Session session, PlayerInfo request){
		await HandlersHelperBase.Instance.PlayerInfoHandler(session, request);
	}
	}
	[MessageHandler]
	public class C2G_PlayerInfoHandler : AMRpcHandler<C2G_PlayerInfo, G2C_PlayerInfo>
	{
		 protected override async ETTask Run(Session session, C2G_PlayerInfo request, G2C_PlayerInfo response, Action reply){
		await HandlersHelperBase.Instance.C2G_PlayerInfoHandler(session, request, response, reply);
	}
	}
	[ActorMessageHandler]
	public class UnitOperationHandler : AMActorLocationHandler<Unit, UnitOperation>
	{
		 protected override async ETTask Run(Unit unit, UnitOperation request){
		await HandlersHelperBase.Instance.UnitOperationHandler(unit, request);
	}
	}
	[ActorMessageHandler]
	public class M2C_InViewUnitsHandler : AMActorHandler<Scene, M2C_InViewUnits>
	{
		 protected override async ETTask Run(Scene scene, M2C_InViewUnits request){
		await HandlersHelperBase.Instance.M2C_InViewUnitsHandler(scene, request);
	}
	}
	[ActorMessageHandler]
	public class M2C_OnEnterViewHandler : AMActorHandler<Scene, M2C_OnEnterView>
	{
		 protected override async ETTask Run(Scene scene, M2C_OnEnterView request){
		await HandlersHelperBase.Instance.M2C_OnEnterViewHandler(scene, request);
	}
	}
	[ActorMessageHandler]
	public class M2C_OnLeaveViewHandler : AMActorHandler<Scene, M2C_OnLeaveView>
	{
		 protected override async ETTask Run(Scene scene, M2C_OnLeaveView request){
		await HandlersHelperBase.Instance.M2C_OnLeaveViewHandler(scene, request);
	}
	}
	[ActorMessageHandler]
	public class M2C_OnEntityChangedHandler : AMActorLocationHandler<Unit, M2C_OnEntityChanged>
	{
		 protected override async ETTask Run(Unit unit, M2C_OnEntityChanged request){
		await HandlersHelperBase.Instance.M2C_OnEntityChangedHandler(unit, request);
	}
	}
	[ActorMessageHandler]
	public class C2M_SetEntityPropertyHandler : AMActorLocationHandler<Unit, C2M_SetEntityProperty>
	{
		 protected override async ETTask Run(Unit unit, C2M_SetEntityProperty request){
		await HandlersHelperBase.Instance.C2M_SetEntityPropertyHandler(unit, request);
	}
	}
	[ActorMessageHandler]
	public class FireRequestHandler : AMActorLocationRpcHandler<Unit, FireRequest, MessageResponse>
	{
		 protected override async ETTask Run(Unit unit, FireRequest request, MessageResponse response, Action reply){
		await HandlersHelperBase.Instance.FireRequestHandler(unit, request, response, reply);
	}
	}
	[MessageHandler]
	public class G2M_GetCopyMapHandler : AMRpcHandler<G2M_GetCopyMap, MessageResponse>
	{
		 protected override async ETTask Run(Session session, G2M_GetCopyMap request, MessageResponse response, Action reply){
		await HandlersHelperBase.Instance.G2M_GetCopyMapHandler(session, request, response, reply);
	}
	}
}
