using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Joints;
using UnityEngine;

namespace ETModel
{
	[ObjectSystem]
	public class Body2dComponentAwakeSystem : AwakeSystem<Body2dComponent>
	{
		public override void Awake(Body2dComponent self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class Body2dComponentUpdateSystem : UpdateSystem<Body2dComponent>
	{
		public override void Update(Body2dComponent self)
		{
			self.Update();
		}
	}

	public class Body2dComponent : Entity
	{
		public Body Body { get; set; }
		public Vector2 Position
		{
			get
			{
				return GetParent<Entity>().GetComponent<TransformComponent>().Position;
			}
		}
		public float Angle
		{
			get
			{
				return GetParent<Entity>().GetComponent<TransformComponent>().Rotation.y;
			}
		}


		public void Awake()
		{
			Body = Test.Instance.CreateBoxCollider(0, 0, 2, 2);
		}

		public void Update()
		{
			Body.SetTransform(new System.Numerics.Vector2(Position.x, Position.y), Angle);
		}
	}
}