using ETModel;
using PF;
using Vector3 = UnityEngine.Vector3;
using UnityEngine;

namespace ETHotfix
{
    /// <summary>
    /// 首次进入场景
    /// </summary>
    [MessageHandler]
    public class M2C_InViewUnitsHandler : AMHandler<M2C_InViewUnits>
    {
        protected override async ETTask Run(ETModel.Session session, M2C_InViewUnits message)
        {
            CreateLocalUnit(message);
            CreateRemoteUnits(message);
            await ETTask.CompletedTask;
        }

        //创建本地玩家
        private void CreateLocalUnit(M2C_InViewUnits message)
        {
            var localUnit = MongoHelper.FromBson<Unit>(message.SelfUnit.bytes);
            Unit.LocalUnit = localUnit;
            Log.Debug($"localUnit.Position {localUnit.Position} ");
            Log.Debug($"localUnit.Position {localUnit.Components.Count} ");
            //Log.Debug($"localUnit.Position  {localUnit.GetComponent<TransformComponent>()}");
            var go = UnityEngine.Object.Instantiate(PrefabHelper.GetUnitPrefab("LocalUnit"));
            go.transform.position = localUnit.Position;
            GameObject.DontDestroyOnLoad(go);
            foreach (var item in localUnit.Components.Values)
            {
                Log.Debug($"{item.GetType().Name}");
                ETModel.Game.EventSystem.RegisterSystem(item);
                item.Parent = localUnit;
            }
            //var unit = ETModel.EntityFactory.CreateWithId<Unit>(ETModel.Game.Scene, localUnit.Id);
            ETModel.Game.EventSystem.Awake(localUnit);
            localUnit.Awake(go);
            UnitComponent.Instance.Add(localUnit);
            //go.transform.GetChild(2).parent = null;
            //go.transform.GetChild(1).parent = null;
            localUnit.Position = localUnit.Position;
            Game.Scene.AddComponent<OperaComponent>();
        }

        //创建其他玩家
        private void CreateRemoteUnits(M2C_InViewUnits message)
        {
            foreach (var entityJson in message.InViewEntitys)
            {
                M2C_OnEnterViewHandler.OnEnterView(entityJson).Coroutine();
            }
        }
    }
}
