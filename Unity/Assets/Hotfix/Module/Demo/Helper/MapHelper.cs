using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class MapHelper
    {
        public static async ETVoid EnterMapAsync(string sceneName)
        {
            try
            {
                // 加载Unit资源
                var resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"unit.unity3d");

                // 加载场景资源
                await ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadBundleAsync("map.unity3d");
                // 切换到map场景
                using (SceneChangeComponent sceneChangeComponent = ETModel.Game.Scene.AddComponent<SceneChangeComponent>())
                {
                    await sceneChangeComponent.ChangeSceneAsync(sceneName);
                }
				
                var g2CEnterMap = await ETModel.SessionComponent.Instance.Session.Call(new C2G_EnterMap()) as G2C_EnterMap;
                PlayerComponent.Instance.MyPlayer.UnitId = g2CEnterMap.UnitId;

                var go = UnityEngine.Object.Instantiate(PrefabHelper.GetUnitPrefab("Unit"));
                GameObject.DontDestroyOnLoad(go);
                var unit = ETModel.EntityFactory.CreateWithId<Unit>(ETModel.Game.Scene, g2CEnterMap.UnitId);
                Unit.LocalUnit = unit;
                unit.Awake(go.transform.GetChild(0).gameObject);
                UnitComponent.Instance.Add(unit);
                go.transform.GetChild(2).parent = null;
                go.transform.GetChild(1).parent = null;

                //var comp = Unit.LocalUnit.ViewGO.GetComponent<KinematicCharacterController.Examples.ExampleCharacterController>();
                //Unit.LocalUnit.ViewGO.GetComponentInChildren<KinematicCharacterController.Examples.ExamplePlayer>().Character = comp;
                Game.Scene.AddComponent<OperaComponent>();

                UnitComponent unitComponent = ETModel.Game.Scene.GetComponent<UnitComponent>();
                foreach (UnitInfo unitInfo in g2CEnterMap.Units)
                {
                    if (unitComponent.Get(unitInfo.UnitId) != null)
                        continue;
                    M2C_CreateUnitsHandler.OnEnterView(unitInfo);
                }

                Game.EventSystem.Run(EventIdType.EnterMapFinish);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }	
        }

        public static async ETTask FireRequest(Vector3 point)
        {
            try
            {
                var msg = new FireRequest();
                msg.X = (int)(point.x * 100);
                msg.Y = (int)(point.y * 100);
                msg.Z = (int)(point.z * 100);
                SessionHelper.HotfixSend(msg);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}