using System;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
	[ComponentDefine]
	public class HealthComponent : Entity, ISerializeToEntity
	{
		public const string DeadEvent = nameof(DeadEvent);
		public ReactProperty<int> HPProperty { get; } = new ReactProperty<int>(100);
		[BsonIgnore]
		[PropertyDefine(SyncFlag.AllClients)]
		public int HP { get { return HPProperty.Value; } set { HPProperty.Value = value; PublishProperty(nameof(HP), value); } }

		public ReactProperty<int> StateProperty { get; } = new ReactProperty<int>(1);
		[BsonIgnore]
		[PropertyDefine(SyncFlag.AllClients)]
		public int State { get { return StateProperty.Value; } set { StateProperty.Value = value; PublishProperty(nameof(State), value); } }


		public void DoDamage(int damage)
		{
			HP = Mathf.Max(0, HP - damage);
			Log.Debug($"DoDamage {damage}");
			if (HP <= 0)
			{
				Dead();
			}
		}

		public void Dead()
		{
			State = 0;
			if (Parent is Unit unit) unit.Dead(null);
			//if (Parent is Monster monster) monster.Dead(null);
			//Game.EventSystem.Run(DeadEvent, this);
		}

		public async ETTask ReliveLater()
		{
			await TimerComponent.Instance.WaitAsync(5);
			State = 1;
			HP = 100;
		}
	}
}