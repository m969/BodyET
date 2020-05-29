using System;
using System.Threading;
using UnityEngine;

namespace ETModel
{
	[ComponentDefine]
	public class HealthComponent : Entity, ISerializeToEntity
	{
		public ReactProperty<int> HPProperty { get; } = new ReactProperty<int>();
		[PropertyDefine(SyncFlag.AllClients)]
		public int HP { get { return HPProperty.Value; } set { HPProperty.Value = value; PublishProperty(nameof(HP), value); } }

		public void DoDamage(int damage)
		{
			HP = Mathf.Max(0, HP - damage);
		}
	}
}