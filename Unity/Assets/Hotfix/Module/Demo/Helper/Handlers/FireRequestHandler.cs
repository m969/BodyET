using ETModel;
using Vector3 = UnityEngine.Vector3;

namespace ETHotfix
{
	[MessageHandler]
	public class FireRequestHandler : AMHandler<FireRequest>
	{
		protected override async ETTask Run(ETModel.Session session, FireRequest message)
		{
			Log.Debug($"FireRequestHandler {message}");

			await ETTask.CompletedTask;
		}
	}
}
