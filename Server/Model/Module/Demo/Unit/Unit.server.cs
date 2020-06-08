using UnityEngine;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ETModel
{
	public partial class Unit
	{
		[BsonIgnore]
		public long IdleTimer { get; set; }


		public void Setup()
		{
			Awake();
			AddComponent<HealthComponent>();
			Position = new Vector3(0, 0, 0);
			LastPosition = new Vector3(0, 0, 0);
		}

		public void Dead(List<object> param) 
		{
			Log.Debug($"Unit Dead");
			HealthComponent.ReliveLater().Coroutine();
		}

		public async ETTask ReliveLater()
		{
			await TimerComponent.Instance.WaitAsync(5);
			State = 1;
			HealthComponent.HP = 100;
		}
	}
}