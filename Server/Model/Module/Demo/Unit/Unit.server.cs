using UnityEngine;

namespace ETModel
{
	public partial class Unit
	{
		[MongoDB.Bson.Serialization.Attributes.BsonIgnore]
		public Vector3 Position
		{
			get
			{
				if (Transform == null)
					return Vector3.zero;
				return Transform.Position;
			}
			set
			{
				Transform.Position = value;
			}
		}

		public void Initialize()
		{
			var unit = this;
			unit.Awake();
			unit.AddComponent<MoveComponent>();
			unit.AddComponent<TransformComponent>();
			unit.AddComponent<Body2dComponent>().CreateBody(.6f, .6f);
			unit.AddComponent<UnitPathComponent>();
			unit.Position = new Vector3(-10, 0, -10);
		}

		public void Update()
		{

		}

		public void Dead()
		{
			Log.Debug("Dead");
			State = 0;
			ReliveLater().Coroutine();
		}

		public async ETTask ReliveLater()
		{
			await TimerComponent.Instance.WaitAsync(5);
			State = 1;
			HP = 100;
		}
	}
}