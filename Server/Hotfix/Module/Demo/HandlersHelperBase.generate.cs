using System;
using ETModel;
namespace ETHotfix
{
	public class HandlersHelperBase
	{
		public static HandlersHelperBase Instance { get; set; }
		public virtual async ETTask C2R_LoginHandler(ETModel.Session session, C2R_Login request, R2C_Login response, Action reply){}
		public virtual async ETTask C2G_LoginGateHandler(ETModel.Session session, C2G_LoginGate request, G2C_LoginGate response, Action reply){}
		public virtual async ETTask G2C_TestHotfixMessageHandler(ETModel.Session session, G2C_TestHotfixMessage message){}
		public virtual async ETTask C2M_TestActorRequestHandler(Unit unit, C2M_TestActorRequest request, M2C_TestActorResponse response, Action reply){}
		public virtual async ETTask PlayerInfoHandler(ETModel.Session session, PlayerInfo message){}
		public virtual async ETTask C2G_PlayerInfoHandler(ETModel.Session session, C2G_PlayerInfo request, G2C_PlayerInfo response, Action reply){}
		public virtual async ETTask UnitOperationHandler(Unit unit, UnitOperation message){}
		public virtual async ETTask M2C_InViewUnitsHandler(Scene scene, M2C_InViewUnits message){}
		public virtual async ETTask M2C_OnEnterViewHandler(Scene scene, M2C_OnEnterView message){}
		public virtual async ETTask M2C_OnLeaveViewHandler(Scene scene, M2C_OnLeaveView message){}
		public virtual async ETTask M2C_OnEntityChangedHandler(Unit unit, M2C_OnEntityChanged message){}
		public virtual async ETTask C2M_SetEntityPropertyHandler(Unit unit, C2M_SetEntityProperty message){}
		public virtual async ETTask FireRequestHandler(Unit unit, FireRequest request, MessageResponse response, Action reply){}
		public virtual async ETTask G2M_GetCopyMapHandler(ETModel.Session session, G2M_GetCopyMap request, MessageResponse response, Action reply){}
		public virtual async ETTask M2M_TrasferUnitRequestHandler(Scene scene, M2M_TrasferUnitRequest request, M2M_TrasferUnitResponse response, Action reply){}
		public virtual async ETTask M2A_ReloadHandler(Scene scene, M2A_Reload request, A2M_Reload response, Action reply){}
		public virtual async ETTask G2G_LockRequestHandler(Scene scene, G2G_LockRequest request, G2G_LockResponse response, Action reply){}
		public virtual async ETTask G2G_LockReleaseRequestHandler(Scene scene, G2G_LockReleaseRequest request, G2G_LockReleaseResponse response, Action reply){}
		public virtual async ETTask ObjectAddRequestHandler(Scene scene, ObjectAddRequest request, ObjectAddResponse response, Action reply){}
		public virtual async ETTask ObjectLockRequestHandler(Scene scene, ObjectLockRequest request, ObjectLockResponse response, Action reply){}
		public virtual async ETTask ObjectUnLockRequestHandler(Scene scene, ObjectUnLockRequest request, ObjectUnLockResponse response, Action reply){}
		public virtual async ETTask ObjectRemoveRequestHandler(Scene scene, ObjectRemoveRequest request, ObjectRemoveResponse response, Action reply){}
		public virtual async ETTask ObjectGetRequestHandler(Scene scene, ObjectGetRequest request, ObjectGetResponse response, Action reply){}
		public virtual async ETTask R2G_GetLoginKeyHandler(Scene scene, R2G_GetLoginKey request, G2R_GetLoginKey response, Action reply){}
		public virtual async ETTask G2M_CreateUnitHandler(Scene scene, G2M_CreateUnit request, M2G_CreateUnit response, Action reply){}
		public virtual async ETTask G2M_SessionDisconnectHandler(Unit unit, G2M_SessionDisconnect message){}
		public virtual async ETTask C2M_TestRequestHandler(Unit unit, C2M_TestRequest request, M2C_TestResponse response, Action reply){}
		public virtual async ETTask Actor_TransferRequestHandler(Unit unit, Actor_TransferRequest request, Actor_TransferResponse response, Action reply){}
		public virtual async ETTask C2G_EnterMapHandler(ETModel.Session session, C2G_EnterMap request, G2C_EnterMap response, Action reply){}
		public virtual async ETTask M2C_CreateUnitsHandler(Scene scene, M2C_CreateUnits message){}
		public virtual async ETTask Frame_ClickMapHandler(Unit unit, Frame_ClickMap message){}
		public virtual async ETTask M2C_PathfindingResultHandler(Scene scene, M2C_PathfindingResult message){}
		public virtual async ETTask C2R_PingHandler(ETModel.Session session, C2R_Ping request, R2C_Ping response, Action reply){}
		public virtual async ETTask G2C_TestHandler(ETModel.Session session, G2C_Test message){}
		public virtual async ETTask C2M_ReloadHandler(ETModel.Session session, C2M_Reload request, M2C_Reload response, Action reply){}
	}
	[MessageHandler]
	public class C2R_LoginHandler : AMRpcHandler<C2R_Login, R2C_Login>
	{
		 protected override async ETTask Run(ETModel.Session session, C2R_Login request, R2C_Login response, Action reply){
		await HandlersHelperBase.Instance.C2R_LoginHandler(session, request, response, reply);
	}
	}
	[MessageHandler]
	public class C2G_LoginGateHandler : AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
	{
		 protected override async ETTask Run(ETModel.Session session, C2G_LoginGate request, G2C_LoginGate response, Action reply){
		await HandlersHelperBase.Instance.C2G_LoginGateHandler(session, request, response, reply);
	}
	}
	[MessageHandler]
	public class G2C_TestHotfixMessageHandler : AMHandler<G2C_TestHotfixMessage>
	{
		 protected override async ETTask Run(ETModel.Session session, G2C_TestHotfixMessage message){
		await HandlersHelperBase.Instance.G2C_TestHotfixMessageHandler(session, message);
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
		 protected override async ETTask Run(ETModel.Session session, PlayerInfo message){
		await HandlersHelperBase.Instance.PlayerInfoHandler(session, message);
	}
	}
	[MessageHandler]
	public class C2G_PlayerInfoHandler : AMRpcHandler<C2G_PlayerInfo, G2C_PlayerInfo>
	{
		 protected override async ETTask Run(ETModel.Session session, C2G_PlayerInfo request, G2C_PlayerInfo response, Action reply){
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
		 protected override async ETTask Run(ETModel.Session session, G2M_GetCopyMap request, MessageResponse response, Action reply){
		await HandlersHelperBase.Instance.G2M_GetCopyMapHandler(session, request, response, reply);
	}
	}
	[ActorMessageHandler]
	public class M2M_TrasferUnitRequestHandler : AMActorRpcHandler<Scene, M2M_TrasferUnitRequest, M2M_TrasferUnitResponse>
	{
		 protected override async ETTask Run(Scene scene, M2M_TrasferUnitRequest request, M2M_TrasferUnitResponse response, Action reply){
		await HandlersHelperBase.Instance.M2M_TrasferUnitRequestHandler(scene, request, response, reply);
	}
	}
	[ActorMessageHandler]
	public class M2A_ReloadHandler : AMActorRpcHandler<Scene, M2A_Reload, A2M_Reload>
	{
		 protected override async ETTask Run(Scene scene, M2A_Reload request, A2M_Reload response, Action reply){
		await HandlersHelperBase.Instance.M2A_ReloadHandler(scene, request, response, reply);
	}
	}
	[ActorMessageHandler]
	public class G2G_LockRequestHandler : AMActorRpcHandler<Scene, G2G_LockRequest, G2G_LockResponse>
	{
		 protected override async ETTask Run(Scene scene, G2G_LockRequest request, G2G_LockResponse response, Action reply){
		await HandlersHelperBase.Instance.G2G_LockRequestHandler(scene, request, response, reply);
	}
	}
	[ActorMessageHandler]
	public class G2G_LockReleaseRequestHandler : AMActorRpcHandler<Scene, G2G_LockReleaseRequest, G2G_LockReleaseResponse>
	{
		 protected override async ETTask Run(Scene scene, G2G_LockReleaseRequest request, G2G_LockReleaseResponse response, Action reply){
		await HandlersHelperBase.Instance.G2G_LockReleaseRequestHandler(scene, request, response, reply);
	}
	}
	[ActorMessageHandler]
	public class ObjectAddRequestHandler : AMActorRpcHandler<Scene, ObjectAddRequest, ObjectAddResponse>
	{
		 protected override async ETTask Run(Scene scene, ObjectAddRequest request, ObjectAddResponse response, Action reply){
		await HandlersHelperBase.Instance.ObjectAddRequestHandler(scene, request, response, reply);
	}
	}
	[ActorMessageHandler]
	public class ObjectLockRequestHandler : AMActorRpcHandler<Scene, ObjectLockRequest, ObjectLockResponse>
	{
		 protected override async ETTask Run(Scene scene, ObjectLockRequest request, ObjectLockResponse response, Action reply){
		await HandlersHelperBase.Instance.ObjectLockRequestHandler(scene, request, response, reply);
	}
	}
	[ActorMessageHandler]
	public class ObjectUnLockRequestHandler : AMActorRpcHandler<Scene, ObjectUnLockRequest, ObjectUnLockResponse>
	{
		 protected override async ETTask Run(Scene scene, ObjectUnLockRequest request, ObjectUnLockResponse response, Action reply){
		await HandlersHelperBase.Instance.ObjectUnLockRequestHandler(scene, request, response, reply);
	}
	}
	[ActorMessageHandler]
	public class ObjectRemoveRequestHandler : AMActorRpcHandler<Scene, ObjectRemoveRequest, ObjectRemoveResponse>
	{
		 protected override async ETTask Run(Scene scene, ObjectRemoveRequest request, ObjectRemoveResponse response, Action reply){
		await HandlersHelperBase.Instance.ObjectRemoveRequestHandler(scene, request, response, reply);
	}
	}
	[ActorMessageHandler]
	public class ObjectGetRequestHandler : AMActorRpcHandler<Scene, ObjectGetRequest, ObjectGetResponse>
	{
		 protected override async ETTask Run(Scene scene, ObjectGetRequest request, ObjectGetResponse response, Action reply){
		await HandlersHelperBase.Instance.ObjectGetRequestHandler(scene, request, response, reply);
	}
	}
	[ActorMessageHandler]
	public class R2G_GetLoginKeyHandler : AMActorRpcHandler<Scene, R2G_GetLoginKey, G2R_GetLoginKey>
	{
		 protected override async ETTask Run(Scene scene, R2G_GetLoginKey request, G2R_GetLoginKey response, Action reply){
		await HandlersHelperBase.Instance.R2G_GetLoginKeyHandler(scene, request, response, reply);
	}
	}
	[ActorMessageHandler]
	public class G2M_CreateUnitHandler : AMActorRpcHandler<Scene, G2M_CreateUnit, M2G_CreateUnit>
	{
		 protected override async ETTask Run(Scene scene, G2M_CreateUnit request, M2G_CreateUnit response, Action reply){
		await HandlersHelperBase.Instance.G2M_CreateUnitHandler(scene, request, response, reply);
	}
	}
	[ActorMessageHandler]
	public class G2M_SessionDisconnectHandler : AMActorLocationHandler<Unit, G2M_SessionDisconnect>
	{
		 protected override async ETTask Run(Unit unit, G2M_SessionDisconnect request){
		await HandlersHelperBase.Instance.G2M_SessionDisconnectHandler(unit, request);
	}
	}
	[ActorMessageHandler]
	public class C2M_TestRequestHandler : AMActorLocationRpcHandler<Unit, C2M_TestRequest, M2C_TestResponse>
	{
		 protected override async ETTask Run(Unit unit, C2M_TestRequest request, M2C_TestResponse response, Action reply){
		await HandlersHelperBase.Instance.C2M_TestRequestHandler(unit, request, response, reply);
	}
	}
	[ActorMessageHandler]
	public class Actor_TransferRequestHandler : AMActorLocationRpcHandler<Unit, Actor_TransferRequest, Actor_TransferResponse>
	{
		 protected override async ETTask Run(Unit unit, Actor_TransferRequest request, Actor_TransferResponse response, Action reply){
		await HandlersHelperBase.Instance.Actor_TransferRequestHandler(unit, request, response, reply);
	}
	}
	[MessageHandler]
	public class C2G_EnterMapHandler : AMRpcHandler<C2G_EnterMap, G2C_EnterMap>
	{
		 protected override async ETTask Run(ETModel.Session session, C2G_EnterMap request, G2C_EnterMap response, Action reply){
		await HandlersHelperBase.Instance.C2G_EnterMapHandler(session, request, response, reply);
	}
	}
	[ActorMessageHandler]
	public class M2C_CreateUnitsHandler : AMActorHandler<Scene, M2C_CreateUnits>
	{
		 protected override async ETTask Run(Scene scene, M2C_CreateUnits request){
		await HandlersHelperBase.Instance.M2C_CreateUnitsHandler(scene, request);
	}
	}
	[ActorMessageHandler]
	public class Frame_ClickMapHandler : AMActorLocationHandler<Unit, Frame_ClickMap>
	{
		 protected override async ETTask Run(Unit unit, Frame_ClickMap request){
		await HandlersHelperBase.Instance.Frame_ClickMapHandler(unit, request);
	}
	}
	[ActorMessageHandler]
	public class M2C_PathfindingResultHandler : AMActorHandler<Scene, M2C_PathfindingResult>
	{
		 protected override async ETTask Run(Scene scene, M2C_PathfindingResult request){
		await HandlersHelperBase.Instance.M2C_PathfindingResultHandler(scene, request);
	}
	}
	[MessageHandler]
	public class C2R_PingHandler : AMRpcHandler<C2R_Ping, R2C_Ping>
	{
		 protected override async ETTask Run(ETModel.Session session, C2R_Ping request, R2C_Ping response, Action reply){
		await HandlersHelperBase.Instance.C2R_PingHandler(session, request, response, reply);
	}
	}
	[MessageHandler]
	public class G2C_TestHandler : AMHandler<G2C_Test>
	{
		 protected override async ETTask Run(ETModel.Session session, G2C_Test message){
		await HandlersHelperBase.Instance.G2C_TestHandler(session, message);
	}
	}
	[MessageHandler]
	public class C2M_ReloadHandler : AMRpcHandler<C2M_Reload, M2C_Reload>
	{
		 protected override async ETTask Run(ETModel.Session session, C2M_Reload request, M2C_Reload response, Action reply){
		await HandlersHelperBase.Instance.C2M_ReloadHandler(session, request, response, reply);
	}
	}
}
