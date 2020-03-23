using ETModel;
namespace ETHotfix
{
	[Message(HotfixOpcode.C2R_Login)]
	public partial class C2R_Login : IRequest {}

	[Message(HotfixOpcode.R2C_Login)]
	public partial class R2C_Login : IResponse {}

	[Message(HotfixOpcode.C2G_LoginGate)]
	public partial class C2G_LoginGate : IRequest {}

	[Message(HotfixOpcode.G2C_LoginGate)]
	public partial class G2C_LoginGate : IResponse {}

	[Message(HotfixOpcode.G2C_TestHotfixMessage)]
	public partial class G2C_TestHotfixMessage : IMessage {}

	[Message(HotfixOpcode.MessageResponse)]
	public partial class MessageResponse : IActorLocationResponse {}

	[Message(HotfixOpcode.C2M_TestActorRequest)]
	public partial class C2M_TestActorRequest : IActorLocationRequest {}

	[Message(HotfixOpcode.M2C_TestActorResponse)]
	public partial class M2C_TestActorResponse : IActorLocationResponse {}

	[Message(HotfixOpcode.PlayerInfo)]
	public partial class PlayerInfo : IMessage {}

	[Message(HotfixOpcode.C2G_PlayerInfo)]
	public partial class C2G_PlayerInfo : IRequest {}

	[Message(HotfixOpcode.G2C_PlayerInfo)]
	public partial class G2C_PlayerInfo : IResponse {}

	[Message(HotfixOpcode.UnitInfo)]
	public partial class UnitInfo {}

	[Message(HotfixOpcode.UnitOperation)]
	public partial class UnitOperation : IActorLocationMessage {}

	[Message(HotfixOpcode.EntiyInfo)]
	public partial class EntiyInfo {}

//当玩家第一次进入某个场景，得到场景内的所有玩家信息
	[Message(HotfixOpcode.M2C_InViewUnits)]
	public partial class M2C_InViewUnits : IActorMessage {}

// repeated string InViewEntitys = 1;
// string SelfUnit = 2;
//当玩家进入视野
	[Message(HotfixOpcode.M2C_OnEnterView)]
	public partial class M2C_OnEnterView : IActorMessage {}

// string EnterEntity = 1;
//当玩家离开视野
	[Message(HotfixOpcode.M2C_OnLeaveView)]
	public partial class M2C_OnLeaveView : IActorMessage {}

	[Message(HotfixOpcode.M2C_OnEntityChanged)]
	public partial class M2C_OnEntityChanged : IActorLocationMessage {}

// repeated int32 TypeParams = 9;
// repeated string ValueParams = 10;
	[Message(HotfixOpcode.FireRequest)]
	public partial class FireRequest : IActorLocationRequest {}

	[Message(HotfixOpcode.G2M_GetCopyMap)]
	public partial class G2M_GetCopyMap : IRequest {}

}
namespace ETHotfix
{
	public static partial class HotfixOpcode
	{
		 public const ushort C2R_Login = 10001;
		 public const ushort R2C_Login = 10002;
		 public const ushort C2G_LoginGate = 10003;
		 public const ushort G2C_LoginGate = 10004;
		 public const ushort G2C_TestHotfixMessage = 10005;
		 public const ushort MessageResponse = 10006;
		 public const ushort C2M_TestActorRequest = 10007;
		 public const ushort M2C_TestActorResponse = 10008;
		 public const ushort PlayerInfo = 10009;
		 public const ushort C2G_PlayerInfo = 10010;
		 public const ushort G2C_PlayerInfo = 10011;
		 public const ushort UnitInfo = 10012;
		 public const ushort UnitOperation = 10013;
		 public const ushort EntiyInfo = 10014;
		 public const ushort M2C_InViewUnits = 10015;
		 public const ushort M2C_OnEnterView = 10016;
		 public const ushort M2C_OnLeaveView = 10017;
		 public const ushort M2C_OnEntityChanged = 10018;
		 public const ushort FireRequest = 10019;
		 public const ushort G2M_GetCopyMap = 10020;
	}
}
