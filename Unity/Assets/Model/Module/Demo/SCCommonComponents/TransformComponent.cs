using System;
using System.Threading;
using UnityEngine;

namespace ETModel
{
	public class TransformComponent : Entity, ISerializeToEntity
	{
		//[MongoDB.Bson.Serialization.Attributes.BsonIgnore]
		public Vector3 LastPosition { get; set; }
		//[MongoDB.Bson.Serialization.Attributes.BsonIgnore]
		public Vector3 Position { get; set; }
		//[MongoDB.Bson.Serialization.Attributes.BsonIgnore]
		public float Rotation { get; set; }
		//[MongoDB.Bson.Serialization.Attributes.BsonIgnore]
		public float Scale { get; set; }
	}
}