using System;
using ETModel;

namespace ETHotfix
{
	public static class Init
	{
		public static void Start()
		{
#if ILRuntime
			if (!Define.IsILRuntime)
			{
				Log.Error("mono层是mono模式, 但是Hotfix层是ILRuntime模式");
			}
#else
			if (Define.IsILRuntime)
			{
				Log.Error("mono层是ILRuntime模式, Hotfix层是mono模式");
			}
#endif
			
			try
			{
				// 注册热更层回调
				ETModel.Game.Hotfix.Update = () => { Update(); };
				ETModel.Game.Hotfix.LateUpdate = () => { LateUpdate(); };
				ETModel.Game.Hotfix.OnApplicationQuit = () => { OnApplicationQuit(); };

				OpcodeHelper.ignoreDebugLogMessageSet.Add(HotfixOpcode.UnitOperation);
				OpcodeHelper.ignoreDebugLogMessageSet.Add(HotfixOpcode.M2C_OnEntityChanged);

				Game.Scene.AddComponent<UIComponent>();
				Game.Scene.AddComponent<OpcodeTypeComponent>();
				Game.Scene.AddComponent<MessageDispatcherComponent>();

				// 加载热更配置
				ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("config.unity3d");
				Game.Scene.AddComponent<ConfigComponent>();
				ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");

				UnitConfig unitConfig = (UnitConfig)Game.Scene.GetComponent<ConfigComponent>().Get(typeof(UnitConfig), 1001);
				Log.Debug($"config {JsonHelper.ToJson(unitConfig)}");

				Game.EventSystem.Run(EventIdType.InitSceneStart);

				EntityDefine.OnPropertyChanged += (entity, name, value) =>
				{
					var propertyDefineCollection = EntityDefine.PropertyDefineCollectionMap[EntityDefine.GetTypeId<Unit>()];
					var attr = propertyDefineCollection[name];
					var msg = new C2M_SetEntityProperty();
					msg.PropertyId = attr.Id;
					msg.PropertyValue.bytes = value;
					SessionHelper.HotfixSend(msg);
				};
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public static void Update()
		{
			try
			{
				Game.EventSystem.Update();
				foreach (var item in BulletComponent.Instance.GetAll())
					BulletHotfix.Update(item);
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public static void LateUpdate()
		{
			try
			{
				Game.EventSystem.LateUpdate();
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public static void OnApplicationQuit()
		{
			Game.Close();
		}
	}
}