using System;
using System.Threading;
using UnityEngine;

namespace ETModel
{
	public class TransformComponent : Entity, ISerializeToEntity
	{
		public Vector3 LastPosition { get; set; }
		public Vector3 Position { get; set; }
		public float Rotation { get; set; }
		public float Scale { get; set; }
	}
}