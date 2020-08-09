using System;
using ETModel;

namespace ETHotfix
{
	public class HandlersHelperBase
	{
		public virtual async ETTask G2M_CreateUnitsHandler(Scene scene, G2M_CreateUnit request, M2G_CreateUnit response, Action reply)
		{
		}
	}

	[ActorMessageHandler]
	public class G2M_CreateUnitsHandler : AMActorRpcHandler<Scene, G2M_CreateUnit, M2G_CreateUnit>
	{
		protected override async ETTask Run(Scene scene, G2M_CreateUnit request, M2G_CreateUnit response, Action reply)
		{
			await MapHandlersHelper.Instance.G2M_CreateUnitsHandler(scene, request, response, reply);
		}
	}
}