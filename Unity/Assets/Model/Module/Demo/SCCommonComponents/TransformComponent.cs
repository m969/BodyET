using System;
using System.Threading;
using UnityEngine;

namespace ETModel
{
	public class TransformComponent : Entity, ISerializeToEntity
	{
		public Vector3 Position { get; set; }
		public Vector3 Rotation { get; set; }
		public Vector3 Scale { get; set; }
	}
}