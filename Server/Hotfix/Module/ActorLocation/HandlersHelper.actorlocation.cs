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
		public override async ETTask ObjectAddRequestHandler(Scene scene, ObjectAddRequest request, ObjectAddResponse response, Action reply)
		{
			await scene.GetComponent<LocationComponent>().Add(request.Key, request.InstanceId);
			reply();
		}

		public override async ETTask ObjectGetRequestHandler(Scene scene, ObjectGetRequest request, ObjectGetResponse response, Action reply)
		{
			long instanceId = await scene.GetComponent<LocationComponent>().Get(request.Key);
			if (instanceId == 0)
			{
				response.Error = ErrorCode.ERR_ActorLocationNotFound;
			}
			response.InstanceId = instanceId;
			reply();
		}

		public override async ETTask ObjectLockRequestHandler(Scene scene, ObjectLockRequest request, ObjectLockResponse response, Action reply)
		{
			scene.GetComponent<LocationComponent>().Lock(request.Key, request.InstanceId, request.Time).Coroutine();
			reply();
			await ETTask.CompletedTask;
		}

		public override async ETTask ObjectRemoveRequestHandler(Scene scene, ObjectRemoveRequest request, ObjectRemoveResponse response, Action reply)
		{
			await scene.GetComponent<LocationComponent>().Remove(request.Key);
			reply();
			await ETTask.CompletedTask;
		}

		public override async ETTask ObjectUnLockRequestHandler(Scene scene, ObjectUnLockRequest request, ObjectUnLockResponse response, Action reply)
		{
			scene.GetComponent<LocationComponent>().UnLock(request.Key, request.OldInstanceId, request.InstanceId);
			reply();
			await ETTask.CompletedTask;
		}
	}
}