using PF;
using UnityEngine;
using KinematicCharacterController.Examples;
using KinematicCharacterController;
using System.Threading.Tasks;

namespace ETModel
{
	public sealed partial class Unit
	{
		public static Unit LocalUnit { get; set; }
		public GameObject BodyView { get; set; }
		public Vector3 LastPosition { get; set; } = Vector3.zero;
		public Transform SkillDiretorTrm { get; set; }
		public KinematicCharacterMotor KinematicCharacterMotor { get; set; }
		//public ExampleCharacterController ExampleCharacterController { get; set; }
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
				KinematicCharacterMotor = BodyView.GetComponent<KinematicCharacterMotor>();
			}
		}

		public void Update()
		{
			if (LocalUnit == this)
				if (SkillDiretorTrm != null)
					SkillDiretorTrm.position = Position;
		}

		public Vector3 Position
		{
			get
			{
				return BodyView.transform.position;
			}
			set
			{
				BodyView.transform.position = value;
			}
		}

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
	}
}