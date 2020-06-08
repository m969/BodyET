using PF;
using UnityEngine;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System;

namespace ETModel
{
	[EntityDefine]
	public partial class Unit: Entity, ITransform
	{
		public ReactProperty<string> NicknameProperty { get; } = new ReactProperty<string>("");
		[PropertyDefine(SyncFlag.AllClients)]
		public string Nickname { get { return NicknameProperty.Value; } set { NicknameProperty.Value = value; PublishProperty(nameof(Nickname), value); } }

		public ReactProperty<int> StateProperty { get; } = new ReactProperty<int>(1);
		[PropertyDefine(SyncFlag.AllClients)]
		public int State { get { return StateProperty.Value; } set { StateProperty.Value = value; PublishProperty(nameof(State), value); } }

		public long PlayerId { get; set; }
		public bool PreviousFiring { get; set; }
		public bool Firing { get; set; }
		[BsonIgnore]
		public ITransform Transform { get { return (this as ITransform); } }
		[BsonIgnore]
		public HealthComponent HealthComponent { get { return GetComponent<HealthComponent>(); } }


		public void Awake()
		{
			Nickname = "";
			//HP = 100;
			State = 1;
			Firing = false;
			//Game.EventSystem.RegisterEvent(HealthComponent.DeadEvent, new EventProxy(Dead));
		}

		public override void Dispose()
		{
			base.Dispose();
		}

		public Vector3 LastPosition { get; set; }
		[BsonIgnore]
		public ReactProperty<Vector3> PositionProperty { get; set; } = new ReactProperty<Vector3>();
		public Vector3 Position
		{
			get
			{
#if !SERVER
				if (BodyView != null)
					return BodyView.transform.position;
#endif
				return PositionProperty.Value;
			}
			set
			{
#if !SERVER
				if (BodyView != null)
					//if (IsLocalUnit)
					//	CharacterMotor.SetPosition(value);
					//else
						BodyView.transform.position = value;
#endif
				PositionProperty.Value = value;
			}
		}
		public float Scale { get; set; }
		public float Angle { get; set; }
	}
}