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
			//Log.Debug($"{JsonHelper.ToJson(message)}");
			MessageHelper.BroadcastToOther(unit, message);
			if (message.Operation == OperaType.Fire)
			{
				var bullet = EntityFactory.Create<Bullet>(unit.Domain);
				var bulletMove = bullet.AddComponent<BulletMoveComponent>();
				var targetPoint = new Vector3();
				targetPoint.x = message.IntParams[0] / 100f;
				targetPoint.y = message.IntParams[1] / 100f;
				targetPoint.z = message.IntParams[2] / 100f;
				bulletMove.MoveToAsync(targetPoint, new ETCancellationToken()).Coroutine();
			}
			await ETTask.CompletedTask;
		}
	}
}