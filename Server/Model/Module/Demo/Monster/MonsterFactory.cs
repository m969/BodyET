using System.Collections.Generic;
using System.Linq;

namespace ETModel
{
	public class MonsterFactory
	{
		public static Monster Create(Entity domain)
		{
			var monster = EntityFactory.Create<Monster>(domain);
			monster.AddComponent<MoveComponent>();
			monster.AddComponent<EntitySyncComponent>();
			monster.AddComponent<HealthComponent>();
			monster.AddComponent<Body2dComponent>().CreateBody(1, 1);
			return monster;
		}
	}
}