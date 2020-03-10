using ETModel;
using PF;
using Vector3 = UnityEngine.Vector3;
using UnityEngine;
using System.Collections.Generic;

namespace ETHotfix
{
	[MessageHandler]
	public class M2C_OnEnterViewHandler : AMHandler<M2C_OnEnterView>
	{
		protected override async ETTask Run(ETModel.Session session, M2C_OnEnterView message)
		{
			OnEnterView(message.EnterUnit);

			await ETTask.CompletedTask;
		}
		
		public static void OnEnterView(UnitInfo unitInfo)
		{
			Unit unit = UnitFactory.Create(ETModel.Game.Scene, unitInfo.UnitId);
			unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);
		}
    }
}
