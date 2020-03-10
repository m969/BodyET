using ETModel;
using PF;
using Vector3 = UnityEngine.Vector3;
using UnityEngine;

namespace ETHotfix
{
	[MessageHandler]
	public class M2C_OnLeaveViewHandler : AMHandler<M2C_OnLeaveView>
	{
		protected override async ETTask Run(ETModel.Session session, M2C_OnLeaveView message)
		{
			await ETTask.CompletedTask;
		}
    }
}
