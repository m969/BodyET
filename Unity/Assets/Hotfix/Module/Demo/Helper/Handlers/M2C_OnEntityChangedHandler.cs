using ETModel;
using Vector3 = UnityEngine.Vector3;

namespace ETHotfix
{
	[MessageHandler]
	public class M2C_OnEntityChangedHandler : AMHandler<M2C_OnEntityChanged>
	{
		protected override async ETTask Run(ETModel.Session session, M2C_OnEntityChanged message)
		{
			Log.Debug($"{message}");
			await ETTask.CompletedTask;
		}
	}
}
