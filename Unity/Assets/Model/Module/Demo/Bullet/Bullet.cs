using UnityEngine;
using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
	[EntityDefine(2)]
	public partial class Bullet : Entity, ITransform
	{
		public long OwnerId { get; set; }
		[BsonIgnore]
		public long Internal { get; set; }
		[BsonIgnore]
		public long Timer { get; set; }

		[BsonIgnore]
		public ITransform Transform { get { return (this as ITransform); } }


		public void Awake()
		{
			OwnerId = 0;
			Internal = 10;
			Timer = 0;
		}

		[BsonIgnore]
		public ReactProperty<Vector3> PositionProperty { get; set; } = new ReactProperty<Vector3>();
		public Vector3 Position
		{
			get
			{
#if SERVER
				//if (Transform != null)
				//	return Transform.Position;
#else
				if (BodyView != null)
					return BodyView.transform.position;
#endif
				return PositionProperty.Value;
			}
			set
			{
#if SERVER
				//if (Transform != null)
				//	Transform.Position = value;
#else
				if (BodyView != null)
					BodyView.transform.position = value;
#endif
				PositionProperty.Value = value;
			}
		}

		public Vector3 LastPosition { get; set; }
		public float Angle			{ get; set; }
		public float Scale			{ get; set; }
	}
}