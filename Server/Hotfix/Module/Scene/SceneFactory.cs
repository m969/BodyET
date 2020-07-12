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
                    //scene.AddComponent<Box2dWorldComponent>();
                    Log.Debug("1");
                    scene.AddComponent<BulletSharpWorldComponent>();
                    Log.Debug("2");
                    scene.AddComponent<UnitComponent>();
                    scene.AddComponent<BulletComponent>();
                    scene.AddComponent<MonsterComponent>();
                    CreateMonsters(scene);
                    //scene.AddComponent<PathfindingComponent>();
                    break;
                case SceneType.Location:
                    scene.AddComponent<LocationComponent>();
                    break;
            }
            await ETTask.CompletedTask;
            return scene;
        }

        private static void CreateMonsters(Scene scene)
        {
            try
            {
                Log.Debug("CreateMonsters");
                var comp = scene.GetComponent<MonsterComponent>();
                Log.Debug("3");
                var monster1 = MonsterFactory.Create(scene);
                Log.Debug("4");
                monster1.Position = new UnityEngine.Vector3(-5, 0, 0);
                monster1.GetComponent<MoveComponent>().Speed = 1f;
                comp.Add(monster1);
                var monster2 = MonsterFactory.Create(scene);
                monster2.GetComponent<MoveComponent>().Speed = 1f;
                monster2.Position = new UnityEngine.Vector3(5, 0, 0);
                comp.Add(monster2);
                monster1.GetComponent<MoveComponent>().MoveTo(monster2.Position).Coroutine();
                monster2.GetComponent<MoveComponent>().MoveTo(monster1.Position).Coroutine();
            }
            catch (System.Exception e)
            {
                Log.Error(e);
            }
        }
    }
}