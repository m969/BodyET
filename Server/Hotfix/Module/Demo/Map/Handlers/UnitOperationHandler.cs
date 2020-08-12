//using ETModel;
//using PF;
//using UnityEngine;

//namespace ETHotfix
//{
//	[ActorMessageHandler]
//	public class UnitOperationHandler : AMActorLocationHandler<Unit, UnitOperation>
//	{
//		protected override async ETTask Run(Unit unit, UnitOperation message)
//		{
//			Log.Msg(message);
//			//Log.Debug($"UnitOperationHandler {message}");
//			unit.Position = new Vector3(message.X / 100f, message.Y / 100f, message.Z / 100f);
//			if (message.Operation == OperaType.Fire)
//			{
//				unit.Fire(message);
//			}
//			MessageHelper.BroadcastToOther(unit, message);
//			await ETTask.CompletedTask;
//		}
//	}
//}