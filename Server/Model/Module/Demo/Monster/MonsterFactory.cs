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
#if !BODY3D
				monster.AddComponent<Body2dComponent>().CreateBody(1, 1);
#else
				monster.AddComponent<Body3dComponent>();
#endif
			}
			catch (System.Exception e)
			{
				Log.Error(e);
			}
			return monster;
		}
	}
}