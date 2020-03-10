using ETModel;
using PF;
using Vector3 = UnityEngine.Vector3;
using UnityEngine;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_InViewUnitsHandler : AMHandler<M2C_InViewUnits>
    {
        protected override async ETTask Run(ETModel.Session session, M2C_InViewUnits message)
        {
            UnitComponent unitComponent = ETModel.Game.Scene.GetComponent<UnitComponent>();
            foreach (UnitInfo unitInfo in message.InViewUnits)
            {
                if (unitComponent.Get(unitInfo.UnitId) != null)
                    continue;
                M2C_OnEnterViewHandler.OnEnterView(unitInfo);
            }

            await ETTask.CompletedTask;
        }
    }
}
