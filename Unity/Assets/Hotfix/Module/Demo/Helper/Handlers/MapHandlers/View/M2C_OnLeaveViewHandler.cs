//using ETModel;
//using PF;
//using Vector3 = UnityEngine.Vector3;
//using UnityEngine;

//namespace ETHotfix
//{
//	[MessageHandler]
//	public class M2C_OnLeaveViewHandler : AMHandler<M2C_OnLeaveView>
//	{
//		protected override async ETTask Run(ETModel.Session session, M2C_OnLeaveView message)
//		{
//			if (message.EntityType == EntityDefine.GetTypeId<Unit>())
//			{
//				UnitComponent.Instance.Remove(message.LeaveEntity);
//			}
//			if (message.EntityType == EntityDefine.GetTypeId<Bullet>())
//			{
//				var bullet = BulletComponent.Instance.Get(message.LeaveEntity);
//				if (bullet != null)
//				{
//					BulletComponent.Instance.Remove(message.LeaveEntity);
//				}
//			}
//			await ETTask.CompletedTask;
//		}
//    }
//}
