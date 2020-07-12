using System;
using System.Threading;
using UnityEngine;

namespace ETModel
{
	public class TransformComponent : Entity, ISerializeToEntity
	{
#if !SERVER
		public Transform transform { get; set; }
#endif
		public Vector3 lastPosition { get; set; } = Vector3.zero;
		public Vector3 position { get; set; } = Vector3.zero;
		public Quaternion rotation { get; set; } = Quaternion.identity;
		public float angle { get; set; } = 0f;


		public void SetPosition(Vector3 position)
		{
			this.position = position;
#if !SERVER
			transform.position = position;
#endif
		}

		public void SetRotation(float angle)
		{
			this.angle = angle;
#if !SERVER
			transform.localEulerAngles = new Vector3(0,angle,0);
#endif
		}
	}
}