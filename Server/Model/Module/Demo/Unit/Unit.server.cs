using UnityEngine;
using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
	public partial class Unit
	{
		//[BsonIgnore]
		//public Vector3 Position
		//{
		//	get
		//	{
		//		if (Transform == null)
		//			return Vector3.zero;
		//		return Transform.Position;
		//	}
		//	set
		//	{
		//		Transform.Position = value;
		//	}
		//}

		//[BsonIgnore]
		//public Vector3 LastPosition { get; set; }
		[BsonIgnore]
		public long IdleTimer { get; set; }


		public void Setup()
		{
			Awake();
			//AddComponent<TransformComponent>();
			Position = new Vector3(-10, 0, -10);
			LastPosition = new Vector3(-10, 0, -10);
			//Pos = new Vector3(-10, 0, -10);
		}

		public void Dead()
		{
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