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
		public ITransform Transform { get { return (this as ITransform); } }


		public void Awake()
		{
			OwnerId = 0;
			Interval = 2000;
			Timer = 0;
			Setup();
		}
		partial void Setup();

		[BsonIgnore]
		public ReactProperty<Vector3> PositionProperty { get; set; } = new ReactProperty<Vector3>(Vector3.zero);
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