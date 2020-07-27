using System;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class UnitHUDComponentAwakeSystem : AwakeSystem<UnitHUDComponent>
    {
	    public override void Awake(UnitHUDComponent self)
	    {
		    self.Awake();
	    }
    }

	[ObjectSystem]
	public class UnitHUDComponentUpdateSystem : UpdateSystem<UnitHUDComponent>
	{
		public override void Update(UnitHUDComponent self)
		{
			self.Update();
		}
	}

	public class UnitHUDComponent : Entity
    {
		public Transform UnitHUDTrm { get; set; }


	    public void Awake()
	    {
			UnitHUDTrm = GetParent<Unit>().BodyView.transform.Find("UnitHUD");
			GetParent<Unit>().GetComponent<HealthComponent>().HPProperty.Subscribe(OnHPChanged);
		}

		public void Update()
        {
			if (UnitHUDTrm == null)
				return;
			UnitHUDTrm.eulerAngles = Camera.main.transform.eulerAngles;
			//UnitHUDTrm.forward = Camera.main.transform.position - UnitHUDTrm.position;
		}

		private void OnHPChanged(int value)
		{
			Log.Debug($"OnHPChanged {value}");
			UnitHUDTrm.GetComponentInChildren<UnityEngine.UI.Image>().fillAmount = value / 100f;
		}
    }
}