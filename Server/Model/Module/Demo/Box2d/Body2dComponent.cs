using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;
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
		public System.Action<Body2dComponent> OnBeginContactAction { get; set; }
		public Body Body { get; set; }
		public Vector2 Position
		{
			get
			{
				var p3 = GetParent<Entity>().GetComponent<TransformComponent>().Position;
				return new Vector2(p3.x, p3.z);
			}
			set
			{
				GetParent<Entity>().GetComponent<TransformComponent>().Position = new Vector3(value.x, 0, value.y);
			}
		}

		public float Angle
		{
			get
			{
				return GetParent<Entity>().GetComponent<TransformComponent>().Rotation;
			}
		}

		public void Awake()
		{
		}

		public Body2dComponent CreateBody(float hx, float hy)
		{
			this.Body = Parent.Domain.GetComponent<Box2dWorldComponent>().CreateBoxCollider(this, Position.x, Position.y, hx, hy);
			return this;
		}

		private Vector2 lastPosition { get; set; }
		public void Update()
		{
			if (Position != lastPosition)
			{
				//Log.Debug("Position = " + Position.ToString());
				lastPosition = Position;
			}
			this.Body.SetTransform(new System.Numerics.Vector2(Position.x, Position.y), Angle);
		}

		public override void Dispose()
		{
			Body.World.DestroyBody(Body);
			base.Dispose();
		}

		public void BeginContact(Contact contact, Body2dComponent other)
		{
			OnBeginContactAction?.Invoke(other);
		}

		public void EndContact(Contact contact)
		{
		}
	}
}