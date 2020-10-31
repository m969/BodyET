using PF;
using UnityEngine;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System;

namespace ETModel
{
	public enum UnitType
	{
		Hero,
		Npc,
		Item,
		Monster,
	}

	[EntityDefine]
	public partial class Unit: Entity, ITransform
	{
		public ReactProperty<string> NicknameProperty { get; } = new ReactProperty<string>("");
		[PropertyDefine(SyncFlag.AllClients)]
		public string Nickname { get { return NicknameProperty.Value; } set { NicknameProperty.Value = value; PublishProperty(nameof(Nickname), value); } }

		public ReactProperty<int> StateProperty { get; } = new ReactProperty<int>(1);
		[PropertyDefine(SyncFlag.AllClients)]
		public int State { get { return StateProperty.Value; } set { StateProperty.Value = value; PublishProperty(nameof(State), value); } }


		public UnitType UnitType { get; private set; }
		//public Vector3 Position { get; set; }
		[BsonIgnore]
		public Quaternion Quaternion { get; set; }
		public string name { get; set; }
		public Vector3 Target { get; set; }
		public bool needSend { get; set; }
		public string triggerType { get; set; }
		public List<Vector3> _movepath { get; set; }
		
		public long PlayerId { get; set; }
		public bool PreviousFiring { get; set; }
		public bool Firing { get; set; }
		[BsonIgnore]
		public TransformComponent TransformComponent { get { return GetComponent<TransformComponent>(); } }
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

		public Vector3 Position
		{
			get
			{
//#if !SERVER
//				if (BodyView != null) return BodyView.transform.position;
//#endif
				return TransformComponent.position;
			}
			set
			{
//#if !SERVER
//				if (BodyView != null) BodyView.transform.position = value;
//#endif
				if (TransformComponent != null) TransformComponent.SetPosition(value);
			}
		}

		public Vector3 LastPosition
		{
			get
			{
				return TransformComponent.lastPosition;
			}
			set
			{
				if (TransformComponent != null) TransformComponent.lastPosition = value;
			}
		}

		public float Angle
		{
			get
			{
				return TransformComponent.angle;
			}
			set
			{
				if (TransformComponent != null) TransformComponent.angle = value;
			}
		}
	}
}