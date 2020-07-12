using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ETModel
{
	public class MonsterFactory
	{
		public static Monster Create(Entity domain)
		{
			Monster monster = null;
			try
			{
				monster = EntityFactory.Create<Monster>(domain);
				monster.AddComponent<TransformComponent>();
				monster.AddComponent<MoveComponent>();
				monster.AddComponent<EntitySyncComponent>();
				monster.AddComponent<HealthComponent>();
				//Log.Debug($"MonsterFactory.Create {monster}");
				monster.AddComponent<Body3dComponent>();
				//monster.AddComponent<Body2dComponent>().CreateBody(1, 1);
			}
			catch (System.Exception e)
			{
				Log.Error(e);
			}
			return monster;
		}
	}
}