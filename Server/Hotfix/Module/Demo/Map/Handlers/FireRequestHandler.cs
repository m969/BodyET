//using ETModel;
//using System;

//namespace ETHotfix
//{
//	[ActorMessageHandler]
//	public class FireRequestHandler : AMActorLocationRpcHandler<Unit, FireRequest, MessageResponse>
//	{
//		protected override async ETTask Run(Unit unit, FireRequest message, MessageResponse response, Action reply)
//		{
//			MessageHelper.Broadcast(unit, message);
//			reply();
//			await ETTask.CompletedTask;
//		}
//	}
//}