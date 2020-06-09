using ETModel;

namespace ETHotfix
{
    public static class SceneFactory
    {
        public static async ETTask<Scene> Create(Entity parent, string name, SceneType sceneType)
        {
            return await Create(parent, IdGenerater.GenerateSceneId(), name, sceneType);
        }
        
        public static async ETTask<Scene> Create(Entity parent, long id, string name, SceneType sceneType)
        {
            Scene scene = EntityFactory.CreateScene(id, name, sceneType);
            scene.Parent = parent;

            scene.AddComponent<MailBoxComponent, MailboxType>(MailboxType.UnOrderMessageDispatcher);

            switch (scene.SceneType)
            {
                case SceneType.Realm:
                    break;
                case SceneType.Gate:
                    scene.AddComponent<PlayerComponent>();
                    scene.AddComponent<GateSessionKeyComponent>();
                    break;
                case SceneType.Map:
                    scene.AddComponent<Box2dWorldComponent>();
                    scene.AddComponent<UnitComponent>();
                    scene.AddComponent<BulletComponent>();
                    var comp = scene.AddComponent<MonsterComponent>();
                    //var monster = MonsterFactory.Create(scene);
                    //monster.Position = new UnityEngine.Vector3(2, 0, 2);
                    //comp.Add(monster);
                    //monster = MonsterFactory.Create(scene);
                    //monster.Position = new UnityEngine.Vector3(3, 0, 3);
                    //comp.Add(monster);
                    scene.AddComponent<PathfindingComponent>();
                    break;
                case SceneType.Location:
                    scene.AddComponent<LocationComponent>();
                    break;
            }

            return scene;
        }
    }
}