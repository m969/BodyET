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
	    public int MapMask { get; set; }
		private Vector3 _lastDirection { get; set; }

	    public void Awake()
	    {
		    this.MapMask = LayerMask.GetMask("Map");
			_lastDirection = Vector3.zero;
		}

	    //private readonly Frame_ClickMap frameClickMap = new Frame_ClickMap();
	    private readonly UnitOperation msg = new UnitOperation();
		private long lastSendTime;
		public void Update()
        {
			if (Unit.LocalUnit == null)
				return;
			var localUnit = Unit.LocalUnit;

			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 1000, this.MapMask))
			{
				if (localUnit.SkillDiretorTrm == null)
				{
					localUnit.SkillDiretorTrm = localUnit.BodyView.transform.parent.Find("SkillDirector");
				}
				localUnit.SkillDiretorTrm.position = localUnit.BodyView.transform.position;
				var direction = hit.point - localUnit.SkillDiretorTrm.position;
				var dist = Vector3.Distance(direction, _lastDirection);
				if (dist > 0.5f)
				{
					localUnit.SkillDiretorTrm.forward = _lastDirection = direction;
				}
			}


			msg.UnitId = localUnit.Id;
			//operationMsg.Index++;
			msg.Operation = 0;
			var p = localUnit.Position;
			msg.X = (int)(p.x * 100);
			msg.Y = (int)(p.y * 100);
			msg.Z = (int)(p.z * 100);
			msg.AngleY = (int)(localUnit.BodyView.transform.eulerAngles.y * 100);

			if (Input.GetMouseButtonDown(0))
			{
				localUnit.Firing = true;
			}
			if (Input.GetMouseButtonUp(0))
			{
				localUnit.Firing = false;
			}
			if (localUnit.Firing)
			{
				if (TimeHelper.Now() - localUnit.LastFireTime < 200)
					return;
				localUnit.LastFireTime = TimeHelper.Now();

				localUnit.BodyView.transform.forward = hit.point;
				localUnit.KinematicCharacterMotor.SetRotation(localUnit.SkillDiretorTrm.rotation, false);
				msg.Operation = OperaType.Fire;
				msg.AngleY = (int)(localUnit.BodyView.transform.eulerAngles.y * 100);
				p = localUnit.SkillDiretorTrm.Find("TargetPoint").position;
				msg.IntParams.Clear();
				msg.LongParams.Clear();
				var x = (int)(p.x * 100);
				var y = (int)(p.y * 100);
				var z = (int)(p.z * 100);
				msg.IntParams.Add(x);
				msg.IntParams.Add(y);
				msg.IntParams.Add(z);
				var bulletId = IdGenerater.GenerateId();
				msg.IntParams.Add(1);
				msg.LongParams.Add(bulletId);
				SessionHelper.HotfixSend(msg);
				//var bulletObj = localUnit.LocalFire(p, 1, bulletId);
				return;
			}

			if (TimeHelper.Now() - lastSendTime > 100)
			{
				lastSendTime = TimeHelper.Now();
				if (Vector3.Distance(localUnit.LastPosition, p) < 0.05f)
					return;
				localUnit.LastPosition = p;
				SessionHelper.HotfixSend(msg);
			}
		}
    }
}
