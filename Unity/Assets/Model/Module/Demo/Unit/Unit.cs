using PF;
using UnityEngine;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
	[EntityDefine(1)]
	public partial class Unit: Entity
	{
		[BsonIgnore]
		public ReactProperty<string> NicknameProperty { get; } = new ReactProperty<string>("");
		[PropertyDefine(101, SyncFlag.AllClients)]
		public string Nickname { get { return NicknameProperty.Value; } set { NicknameProperty.Value = value; PublishProperty(nameof(Nickname), value); } }

		[BsonIgnore]
		public ReactProperty<int> HPProperty { get; } = new ReactProperty<int>();
		[PropertyDefine(102, SyncFlag.AllClients)]
		public int HP { get { return HPProperty.Value; } set { HPProperty.Value = value; PublishProperty(nameof(HP), value); } }

		[BsonIgnore]
		public ReactProperty<int> StateProperty { get; } = new ReactProperty<int>(1);
		[PropertyDefine(103, SyncFlag.AllClients)]
		public int State { get { return StateProperty.Value; } set { StateProperty.Value = value; PublishProperty(nameof(State), value); } }

		//private float[] posArr = new float[] { 0, 0, 0 };
		//public float[] PosArr
		//{
		//	get
		//	{
		//		if (Transform != null)
		//		{
		//			posArr[0] = Transform.Position.x;
		//			posArr[1] = Transform.Position.y;
		//			posArr[2] = Transform.Position.z;
		//		}

		//		return posArr;
		//	}
		//	set
		//	{
		//		if (Transform != null)
		//		{
		//			Transform.Position = new Vector3(value[0], value[1], value[2]);
		//			posArr[0] = Transform.Position.x;
		//			posArr[1] = Transform.Position.y;
		//			posArr[2] = Transform.Position.z;
		//		}
		//	}
		//}

		public Vector3 LastPosition { get; set; }
		public long PlayerId { get; set; }
		public bool PreviousFiring { get; set; }
		public bool Firing { get; set; }
		[BsonIgnore]
		public TransformComponent Transform { get { return GetComponent<TransformComponent>(); } }


		public void Awake()
		{
			Nickname = "";
			HP = 100;
			State = 1;
			Firing = false;
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			base.Dispose();
		}

		[BsonIgnore]
		public ReactProperty<Vector3> PositionProperty { get; set; } = new ReactProperty<Vector3>();
		public Vector3 Position
		{
			get
			{
#if SERVER
				if (Transform != null)
					return Transform.Position;
#else
				if (BodyView != null)
					return BodyView.transform.position;
#endif
				//return Vector3.zero;
				return PositionProperty.Value;
			}
			set
			{
#if SERVER
				if (Transform != null)
					Transform.Position = value;
#else
				if (BodyView != null)
					CharacterMotor.SetPosition(value);
#endif
				PositionProperty.Value = value;
			}
		}
	}
}