using ETModel;
using Vector3 = UnityEngine.Vector3;

namespace ETHotfix
{
	[MessageHandler]
	public class M2C_CreateUnitsHandler : AMHandler<M2C_CreateUnits>
	{
		protected override async ETTask Run(ETModel.Session session, M2C_CreateUnits message)
		{
			UnitComponent unitComponent = ETModel.Game.Scene.GetComponent<UnitComponent>();
			foreach (var unitInfo in message.Units)
			{
				if (unitComponent.Get(unitInfo.UnitId) != null)
					continue;
				OnEnterView(unitInfo);
			}

			await ETTask.CompletedTask;
		}

		public static void OnEnterView(ETModel.UnitInfo unitInfo)
		{
			Unit unit = UnitFactory.Create(ETModel.Game.Scene, unitInfo.UnitId);
			unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);
		}
	}
}
