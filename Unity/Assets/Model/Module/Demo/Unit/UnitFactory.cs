using UnityEngine;

namespace ETModel
{
    public static class UnitFactory
    {
        public static Unit Create(Entity domain, long id)
        {
			var go = UnityEngine.Object.Instantiate(PrefabHelper.GetUnitPrefab("OtherCharacter"));
            GameObject.DontDestroyOnLoad(go);
			var unit = EntityFactory.CreateWithId<Unit>(domain, id);
            unit.Awake(go);
            UnitComponent.Instance.Add(unit);
            return unit;
        }
    }
}