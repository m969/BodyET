using System;
using ETModel;
using PF;
using UnityEngine;
using System.Linq;
using ETHotfix;

namespace ETHotfix
{
	public partial class HandlersHelper: HandlersHelperBase
	{
		public override async ETTask Actor_TransferRequestHandler(Unit unit, Actor_TransferRequest request, Actor_TransferResponse response, Action reply)
		{
			long unitId = unit.Id;

			// 先在location锁住unit的地址
			await Game.Scene.GetComponent<LocationProxyComponent>().Lock(unitId, unit.InstanceId);

			// 删除unit,让其它进程发送过来的消息找不到actor，重发
			Game.EventSystem.Remove(unitId);

			long instanceId = unit.InstanceId;

			int mapIndex = request.MapIndex;

			StartConfigComponent startConfigComponent = StartConfigComponent.Instance;

			// 传送到map
			StartConfig mapConfig = startConfigComponent.Get(mapIndex);
			var address = mapConfig.GetComponent<InnerConfig>().Address;
			Session session = Game.Scene.GetComponent<NetInnerComponent>().Get(address);

			// 只删除不disponse否则M2M_TrasferUnitRequest无法序列化Unit
			Game.Scene.GetComponent<UnitComponent>().RemoveNoDispose(unitId);
			M2M_TrasferUnitResponse m2m_TrasferUnitResponse = (M2M_TrasferUnitResponse)await session.Call(new M2M_TrasferUnitRequest() { Unit = unit });
			unit.Dispose();

			// 解锁unit的地址,并且更新unit的instanceId
			await Game.Scene.GetComponent<LocationProxyComponent>().UnLock(unitId, instanceId, m2m_TrasferUnitResponse.InstanceId);

			reply();
		}

		public override async ETTask Frame_ClickMapHandler(Unit unit, Frame_ClickMap message)
		{
			Vector3 target = new Vector3(message.X, message.Y, message.Z);
			unit.GetComponent<UnitPathComponent>().MoveTo(target).Coroutine();
			await ETTask.CompletedTask;
		}

		public override async ETTask M2M_TrasferUnitRequestHandler(Scene scene, M2M_TrasferUnitRequest request, M2M_TrasferUnitResponse response, Action reply)
		{
			Unit unit = request.Unit;
			// 将unit加入事件系统
			Log.Debug(MongoHelper.ToJson(request.Unit));
			// 这里不需要注册location，因为unlock会更新位置
			unit.AddComponent<MailBoxComponent>();
			scene.GetComponent<UnitComponent>().Add(unit);
			response.InstanceId = unit.InstanceId;
			reply();
			await ETTask.CompletedTask;
		}
	}
}