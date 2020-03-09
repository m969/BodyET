using ETModel;
using Vector3 = UnityEngine.Vector3;
using UnityEngine;

namespace ETHotfix
{
	[MessageHandler]
	public class UnitOperationHandler : AMHandler<UnitOperation>
	{
		protected override async ETTask Run(ETModel.Session session, UnitOperation message)
		{
			await ETTask.CompletedTask;
			var unit = UnitComponent.Instance.Get(message.UnitId);
			if (unit == null)
				return;
			if (unit.BodyView == null)
				return;
			unit.Position = new Vector3(message.X / 100f, message.Y / 100f, message.Z / 100f);
			unit.Rotation = Quaternion.Euler(0, message.AngleY / 100f, 0);
			if (message.Operation == OperaType.Fire)
			{
				//Log.Debug($"UnitOperationHandler {JsonHelper.ToJson(message)}");
				var x = (int)(message.IntParams[0] / 100f);
				var y = (int)(message.IntParams[1] / 100f);
				var z = (int)(message.IntParams[2] / 100f);
				var bulletType = message.IntParams[3];
				var bulletId = message.LongParams[0];
				unit.Fire(new Vector3(x, y, z), bulletType, bulletId);
			}
		}
	}
}
