using UnityEngine;
using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
	[EntityDefine]
	public partial class Monster : Entity, ITransform
	{
		public long OwnerId { get; set; }
		[BsonIgnore]
		public long Interval { get; set; }
		[BsonIgnore]
		public long Timer { get; set; }

		[BsonIgnore]
		public TransformComponent TransformComponent { get { return GetComponent<TransformComponent>(); } }


		public void Awake()
		{
			OwnerId = 0;
			Interval = 2000;
			Timer = 0;
			Setup();
		}
		partial void Setup();

		//[BsonIgnore]
		//public ReactProperty<Vector3> PositionProperty { get; set; } = new ReactProperty<Vector3>(Vector3.zero);
		public Vector3 Position
		{
			get
			{
//#if !SERVER
//				if (BodyView != null)
//					return BodyView.transform.position;
//#else
				return TransformComponent.position;
//#endif
			}
			set
			{
//#if !SERVER
//				if (BodyView != null)
//					BodyView.transform.position = value;
//#else
				TransformComponent.SetPosition(value);
//#endif
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
				TransformComponent.lastPosition = value;
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
				TransformComponent.angle = value;
			}
		}
	}
}