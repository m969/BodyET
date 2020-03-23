using UnityEngine;
using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
	[EntityDefine(2)]
	public partial class Bullet : Entity
	{
		public long OwnerId { get; set; }
		[BsonIgnore]
		public long Internal { get; set; }
		[BsonIgnore]
		public long Timer { get; set; }

		private float[] posArr = new float[] { 0, 0, 0 };
		[BsonRepresentation(MongoDB.Bson.BsonType.Double)]
		public float[] PosArr
		{
			get
			{
				if (Transform != null)
				{
					posArr[0] = Transform.Position.x;
					posArr[1] = Transform.Position.y;
					posArr[2] = Transform.Position.z;
				}

				return posArr;
			}
			set
			{
				if (Transform != null)
				{
					Transform.Position = new Vector3(value[0], value[1], value[2]);
					posArr[0] = Transform.Position.x;
					posArr[1] = Transform.Position.y;
					posArr[2] = Transform.Position.z;
				}
			}
		}
		[BsonIgnore]
		public TransformComponent Transform { get { return GetComponent<TransformComponent>(); } }


		public void Awake()
		{
			OwnerId = 0;
			Internal = 10;
			Timer = 0;
		}
	}
}