using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ETModel
{
	public class MonsterFactory
	{
		public static Monster Create(Entity domain, long id)
		{
			var monster = EntityFactory.CreateWithId<Monster>(domain, id);
			monster.BodyView = GameObject.Instantiate(PrefabHelper.GetUnitPrefab("Monster"));
			GameObject.DontDestroyOnLoad(monster.BodyView);
			return monster;
		}
	}
}