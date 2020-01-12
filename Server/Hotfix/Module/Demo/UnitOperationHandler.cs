using ETModel;
using PF;
using UnityEngine;

namespace ETHotfix
{
	[ActorMessageHandler]
	public class UnitOperationHandler : AMActorLocationHandler<Unit, UnitOperation>
	{
		protected override async ETTask Run(Unit unit, UnitOperation message)
		{
			Vector3 target = new Vector3(message.X, message.Y, message.Z);
			Log.Debug(target.ToString());
			//unit.GetComponent<UnitPathComponent>().MoveTo(target).Coroutine();
			await ETTask.CompletedTask;
		}
	}
}