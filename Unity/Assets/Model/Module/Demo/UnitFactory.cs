using UnityEngine;

namespace ETModel
{
    public static class UnitFactory
    {
        public static Unit Create(Entity domain, long id)
        {
	        //var resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
	        //var bundleGameObject = (GameObject)resourcesComponent.GetAsset("Unit.unity3d", "Unit");
	        //var prefab = bundleGameObject.Get<GameObject>("Skeleton");

			//var go = UnityEngine.Object.Instantiate(prefab);
			var unit = EntityFactory.CreateWithId<Unit, GameObject>(domain, id, GameObject.Find("ExampleCharacter"));

			//unit.AddComponent<AnimatorComponent>();
			//unit.AddComponent<MoveComponent>();
			//unit.AddComponent<TurnComponent>();
			//unit.AddComponent<UnitPathComponent>();

			UnitComponent.Instance.Add(unit);
            return unit;
        }
    }
}