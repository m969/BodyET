using UnityEngine;
using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
	[EntityDefine]
	public partial class Bullet : Entity, ITransform
	{
		public long OwnerId { get; set; }
		[BsonIgnore]
		public TransformComponent TransformComponent { get { return GetComponent<TransformComponent>(); } }


		public void Awake()
		{
			OwnerId = 0;
		}

		//[BsonIgnore]
		//public ReactProperty<Vector3> PositionProperty { get; set; } = new ReactProperty<Vector3>();
		public Vector3 Position
		{
			get
			{
//#if !SERVER
//				if (BodyView != null)
//					return BodyView.transform.position;
//#endif
				return TransformComponent.position;
			}
			set
			{
//#if !SERVER
//				if (BodyView != null)
//					BodyView.transform.position = value;
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