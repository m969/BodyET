using UnityEngine;
using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
	public partial class Monster
	{
		[BsonIgnore]
		public long IdleTimer { get; set; }


		partial void Setup()
		{
			Position = new Vector3(-10, 0, -10);
			LastPosition = new Vector3(-10, 0, -10);
		}
	}
}