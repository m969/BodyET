using ETModel;
using Vector3 = UnityEngine.Vector3;
using UnityEngine;
using DG.Tweening;

namespace ETHotfix
{
	[MessageHandler]
	public class UnitOperationHandler : AMHandler<UnitOperation>
	{
		protected override async ETTask Run(ETModel.Session session, UnitOperation message)
		{
			var unit = UnitComponent.Instance.Get(message.UnitId);
			if (unit == null)
				return;
			if (unit.BodyView == null)
				return;
			var newPosition = new Vector3(message.X / 100f, message.Y / 100f, message.Z / 100f);
			//unit.KinematicCharacterMotor.MoveCharacter(newPosition);
			unit.BodyView.transform.DOMove(newPosition, 0.2f);
			unit.Rotation = new Vector3(0, message.AngleY / 100f, 0);
			//if (message.Operation == OperaType.Fire)
			//{
			//	var x = (int)(message.IntParams[0] / 100f);
			//	var y = (int)(message.IntParams[1] / 100f);
			//	var z = (int)(message.IntParams[2] / 100f);
			//	var bulletType = message.IntParams[3];
			//	var bulletId = message.LongParams[0];
			//	unit.Fire(new Vector3(x, y, z), bulletType, bulletId);
			//}
			await ETTask.CompletedTask;
		}
	}
}
