using UnityEngine;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ETModel
{
	public partial class Monster
	{
		[BsonIgnore]
		public long IdleTimer { get; set; }


		partial void Setup()
		{
			Position = new Vector3(-10, 0, -10);
			LastPosition = new Vector3(-10, 0, -10);
			//Game.EventSystem.RegisterEvent(HealthComponent.DeadEvent, new EventProxy(Dead));
		}

		public void Dead(List<object> param)
		{
			Log.Debug($"Monster Dead");
			MonsterComponent.Instance.Remove(Id);
		}
	}
}