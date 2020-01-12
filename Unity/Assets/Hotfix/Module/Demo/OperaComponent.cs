using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class OperaComponentAwakeSystem : AwakeSystem<OperaComponent>
    {
	    public override void Awake(OperaComponent self)
	    {
		    self.Awake();
	    }
    }

	[ObjectSystem]
	public class OperaComponentUpdateSystem : UpdateSystem<OperaComponent>
	{
		public override void Update(OperaComponent self)
		{
			self.Update();
		}
	}

	public class OperaComponent: Entity
    {
        public Vector3 ClickPoint;

	    public int mapMask;

	    public void Awake()
	    {
		    this.mapMask = LayerMask.GetMask("Map");
	    }

	    private readonly Frame_ClickMap frameClickMap = new Frame_ClickMap();
	    private readonly UnitOperation operationMsg = new UnitOperation();
		private long lastSendTime;
		public void Update()
        {
			if (TimeHelper.Now() - lastSendTime > 100)
			{
				lastSendTime = TimeHelper.Now();
				if (Unit.LocalUnit == null)
					return;
				operationMsg.Index++;
				operationMsg.Operation = 0;
				var p = Unit.LocalUnit.Position;
				operationMsg.X = (int)p.x;
				operationMsg.Y = (int)p.y;
				operationMsg.Z = (int)p.z;
				ETModel.SessionComponent.Instance.Session.Send(operationMsg);
			}
			//        if (Input.GetMouseButtonDown(1))
			//        {
			//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//            RaycastHit hit;
			//         if (Physics.Raycast(ray, out hit, 1000, this.mapMask))
			//         {
			//	this.ClickPoint = hit.point;
			//          frameClickMap.X = this.ClickPoint.x;
			//          frameClickMap.Y = this.ClickPoint.y;
			//          frameClickMap.Z = this.ClickPoint.z;
			//          ETModel.SessionComponent.Instance.Session.Send(frameClickMap);

			//	// 测试actor rpc消息
			//	this.TestActor().Coroutine();
			//}
			//        }
		}

	    public async ETVoid TestActor()
	    {
		    try
		    {
			    M2C_TestActorResponse response = (M2C_TestActorResponse)await SessionComponent.Instance.Session.Call(
						new C2M_TestActorRequest() { Info = "actor rpc request" });
			    Log.Info(response.Info);
			}
		    catch (Exception e)
		    {
				Log.Error(e);
		    }
		}
    }
}
