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