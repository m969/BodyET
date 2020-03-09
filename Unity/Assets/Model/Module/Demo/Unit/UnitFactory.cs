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
			//unit.AddComponent<AnimatorComponent>();
			//unit.AddComponent<MoveComponent>();
			//unit.AddComponent<TurnComponent>();
			//unit.AddComponent<UnitPathComponent>();
			UnitComponent.Instance.Add(unit);
            return unit;
        }
    }
}