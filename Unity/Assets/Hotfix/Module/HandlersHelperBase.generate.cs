using System;
using ETModel;
namespace ETHotfix
{
	public class HandlersHelperBase
	{
		public static HandlersHelperBase Instance { get; set; }
		public virtual async ETTask G2C_TestHotfixMessageHandler(ETModel.Session session, G2C_TestHotfixMessage message){}
		public virtual async ETTask PlayerInfoHandler(ETModel.Session session, PlayerInfo message){}
		public virtual async ETTask UnitOperationHandler(ETModel.Session session, UnitOperation message){}
		public virtual async ETTask M2C_InViewUnitsHandler(ETModel.Session session, M2C_InViewUnits message){}
		public virtual async ETTask M2C_OnEnterViewHandler(ETModel.Session session, M2C_OnEnterView message){}
		public virtual async ETTask M2C_OnLeaveViewHandler(ETModel.Session session, M2C_OnLeaveView message){}
		public virtual async ETTask M2C_OnEntityChangedHandler(ETModel.Session session, M2C_OnEntityChanged message){}
		public virtual async ETTask C2M_SetEntityPropertyHandler(ETModel.Session session, C2M_SetEntityProperty message){}
	}
	[MessageHandler]
	public class G2C_TestHotfixMessageHandler : AMHandler<G2C_TestHotfixMessage>
	{
		 protected override async ETTask Run(ETModel.Session session, G2C_TestHotfixMessage message){
		await HandlersHelperBase.Instance.G2C_TestHotfixMessageHandler(session, message);
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
	public class UnitOperationHandler : AMHandler<UnitOperation>
	{
		 protected override async ETTask Run(ETModel.Session session, UnitOperation message){
		await HandlersHelperBase.Instance.UnitOperationHandler(session, message);
	}
	}
	[MessageHandler]
	public class M2C_InViewUnitsHandler : AMHandler<M2C_InViewUnits>
	{
		 protected override async ETTask Run(ETModel.Session session, M2C_InViewUnits message){
		await HandlersHelperBase.Instance.M2C_InViewUnitsHandler(session, message);
	}
	}
	[MessageHandler]
	public class M2C_OnEnterViewHandler : AMHandler<M2C_OnEnterView>
	{
		 protected override async ETTask Run(ETModel.Session session, M2C_OnEnterView message){
		await HandlersHelperBase.Instance.M2C_OnEnterViewHandler(session, message);
	}
	}
	[MessageHandler]
	public class M2C_OnLeaveViewHandler : AMHandler<M2C_OnLeaveView>
	{
		 protected override async ETTask Run(ETModel.Session session, M2C_OnLeaveView message){
		await HandlersHelperBase.Instance.M2C_OnLeaveViewHandler(session, message);
	}
	}
	[MessageHandler]
	public class M2C_OnEntityChangedHandler : AMHandler<M2C_OnEntityChanged>
	{
		 protected override async ETTask Run(ETModel.Session session, M2C_OnEntityChanged message){
		await HandlersHelperBase.Instance.M2C_OnEntityChangedHandler(session, message);
	}
	}
	[MessageHandler]
	public class C2M_SetEntityPropertyHandler : AMHandler<C2M_SetEntityProperty>
	{
		 protected override async ETTask Run(ETModel.Session session, C2M_SetEntityProperty message){
		await HandlersHelperBase.Instance.C2M_SetEntityPropertyHandler(session, message);
	}
	}
}
