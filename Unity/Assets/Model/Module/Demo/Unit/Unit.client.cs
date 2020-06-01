using PF;
using UnityEngine;
using KinematicCharacterController.Examples;
using KinematicCharacterController;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace ETModel
{
	public sealed partial class Unit
	{
		public static Unit LocalUnit { get; set; }
		public GameObject BodyView { get; set; }
		//public Vector3 LastPosition { get; set; } = Vector3.zero;
		public Transform SkillDiretorTrm { get; set; }
		public KinematicCharacterMotor CharacterMotor { get; set; }
		public ExampleCharacterController CharacterController { get; set; }
		public ETCancellationTokenSource ETCancellationTokenSource { get; set; }
		public long LastFireTime { get; set; }


		public void Awake(GameObject gameObject)
		{
			Awake();
			//this.ViewGO = gameObject;
			//this.ViewGO.AddComponent<ComponentView>().Component = this;
			BodyView = gameObject;
			if (LocalUnit == this)
			{
				SkillDiretorTrm = BodyView.transform.Find("SkillDirector");
				SkillDiretorTrm.parent = null;
				CharacterMotor = BodyView.GetComponent<KinematicCharacterMotor>();
				CharacterController = BodyView.GetComponent<ExampleCharacterController>();
			}
			AddComponent<AnimatorComponent>().Awake(gameObject.GetComponentInChildren<Animator>());
			StateProperty.Subscribe(OnStateChanged);
		}

		public void Update()
		{
			if (LocalUnit == this)
				if (SkillDiretorTrm != null)
					SkillDiretorTrm.position = Position;
		}

		//public Vector3 Position
		//{
		//	get
		//	{
		//		return BodyView.transform.position;
		//	}
		//	set
		//	{
		//		BodyView.transform.position = value;
		//	}
		//}

		public Quaternion Rotation
		{
			get
			{
				return BodyView.transform.rotation;
			}
			set
			{
				BodyView.transform.rotation = value;
			}
		}

		public bool IsLocalUnit
		{
			get
			{
				return LocalUnit == this;
			}
		}

		private void OnStateChanged(int value)
		{
			Log.Debug($"OnStateChanged {value}");
			if (value == 0)
			{
				BodyView.SetActive(false);
			}
			else
			{
				if (BodyView.activeSelf == false)
				{
					BodyView.SetActive(true);
				}
			}
			if (value == 1)
			{
				GetComponent<AnimatorComponent>().Play(MotionType.Idle);
			}
			if (value == 2)
			{
				GetComponent<AnimatorComponent>().Play(MotionType.Run);
			}
		}

		public void Dead(List<object> param)
		{

		}
	}
}