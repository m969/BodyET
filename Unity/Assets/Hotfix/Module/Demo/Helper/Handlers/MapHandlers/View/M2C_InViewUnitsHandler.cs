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
            var selfUnit = MongoHelper.FromBson<Unit>(message.SelfUnit.bytes);
            var go = UnityEngine.Object.Instantiate(PrefabHelper.GetUnitPrefab("Unit"));
            GameObject.DontDestroyOnLoad(go);
            var unit = ETModel.EntityFactory.CreateWithId<Unit>(ETModel.Game.Scene, selfUnit.Id);
            Unit.LocalUnit = unit;
            unit.Awake(go.transform.GetChild(0).gameObject);
            UnitComponent.Instance.Add(unit);
            go.transform.GetChild(2).parent = null;
            go.transform.GetChild(1).parent = null;
            unit.Position = selfUnit.PositionProperty.Value;
            Game.Scene.AddComponent<OperaComponent>();

            foreach (var entityJson in message.InViewEntitys)
            {
                M2C_OnEnterViewHandler.OnEnterView(entityJson).Coroutine();
            }
            await ETTask.CompletedTask;
        }
    }
}
