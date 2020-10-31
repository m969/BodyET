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
		public Box2dWorldComponent Box2DWorldComponent { get; set; }
		public System.Action<Body2dComponent> OnBeginContactAction { get; set; }
		public Body Body { get; set; }
		public Vector2 Position
		{
			get
			{
				var p3 = (GetParent<Entity>() as ITransform).Position;
				return new Vector2(p3.x, p3.z);
			}
			set
			{
				(GetParent<Entity>() as ITransform).Position = new Vector3(value.x, 0, value.y);
			}
		}

		public float Angle
		{
			get
			{
				return (GetParent<Entity>() as ITransform).Angle;
			}
		}

		public void Awake()
		{
		}

		public Body2dComponent CreateBody(float hx, float hy)
		{
			Box2DWorldComponent = Parent.Domain.GetComponent<Box2dWorldComponent>();
			this.Body = Box2DWorldComponent.CreateBoxCollider(this, Position.x, Position.y, hx, hy);
			return this;
		}

		private Vector2 lastPosition { get; set; }
		public void Update()
		{
			if (Parent.GetComponent<TransformComponent>() == null)
            {
				return;
            }
			if (Position != lastPosition)
			{
				//Log.Debug("Position = " + Position.ToString());
				lastPosition = Position;
			}
			this.Body.SetTransform(new System.Numerics.Vector2(Position.x, Position.y), Angle);
		}

		public override void Dispose()
		{
			base.Dispose();
			Box2DWorldComponent.Remove(Body);
		}

		public void BeginContact(Contact contact, Body2dComponent other)
		{
			Log.Debug($"Body2dComponent BeginContact");
            try
            {
				OnBeginContactAction?.Invoke(other);
			}
			catch (System.Exception e)
            {
				Log.Error(e);
            }
		}

		public void EndContact(Contact contact)
		{
		}
	}
}