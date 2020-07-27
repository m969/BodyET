using System;
using System.Threading;
using UnityEngine;
using MongoDB.Bson.Serialization.Attributes;
#if !SERVER
using DG.Tweening;
#endif

namespace ETModel
{
	[ObjectSystem]
	public class TransformComponentUpdateSystem : UpdateSystem<TransformComponent>
	{
		public override void Update(TransformComponent self)
		{
			self.Update();
		}
	}

	public class TransformComponent : Entity, ISerializeToEntity
	{
#if !SERVER
		[BsonIgnore]
		public Transform transform { get; set; }
#endif
		public Vector3 lastPosition { get; set; } = Vector3.zero;
		public Vector3 position { get; set; } = Vector3.zero;
		[BsonIgnore]
		public Quaternion rotation { get; set; } = Quaternion.identity;
		public float angle { get; set; } = 0f;


		public void SetPosition(Vector3 position)
		{
			this.position = position;
#if !SERVER
			if (transform) transform.DOMove(position, 0.2f);
#endif
		}

		public void SetRotation(float angle)
		{
			this.angle = angle;
#if !SERVER
			if (transform) transform.localEulerAngles = new Vector3(0,angle,0);
#endif
		}

		public void Update()
		{
#if !SERVER
			position = transform.position;
#endif
		}
	}
}