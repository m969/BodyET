using ETModel;

namespace ETHotfix
{
	[Event(EventIdType.LoginFinish)]
	public class LoginFinish_CreateLobbyUI: AEvent
	{
		public override void Run()
		{
			MapHelper.EnterMapAsync("Map").Coroutine();
			return;
			UI ui = UILobbyFactory.Create();
			Game.Scene.GetComponent<UIComponent>().Add(ui);
		}
	}
}
