using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.AfterScenesAdd)]
    public class AfterScenesAdd_CreateScene: AEvent
    {
        public override void Run()
        {
            RunInner().Coroutine();
            Box2dCoroutine().Coroutine();
        }

        public async ETVoid RunInner()
        {
            foreach (StartConfig startConfig in StartConfigComponent.Instance.StartConfig.List)
            {
                SceneConfig sceneConfig = startConfig.GetComponent<SceneConfig>();
                Log.Debug(startConfig.ToString());
                if (sceneConfig.SceneType == SceneType.Map)
                    if (startConfig.GetComponent<MapConfig>().MapType == MapType.Copy)
                        continue;
                await SceneFactory.Create(Game.Scene, startConfig.Id, sceneConfig.Name, sceneConfig.SceneType);    
            }
            Unit.OnPropertyChanged = MessageHelper.OnPropertyChanged;
            var unit = EntityFactory.Create<Unit>(Game.Scene);
            Log.Debug($"{JsonHelper.ToJson(unit)}");
        }

        public async ETVoid Box2dCoroutine()
        {
            new Test().Tumbler(false);
            await ETTask.CompletedTask;
        }
    }
}