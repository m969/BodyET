using UnityEngine;

namespace ETModel
{
	public static class PrefabHelper
	{
		public static GameObject GetUnitPrefab(string name)
		{
			var resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
			var bundleGameObject = (GameObject)resourcesComponent.GetAsset("Unit.unity3d", "Unit");
			var prefab = bundleGameObject.Get<GameObject>(name);
			return prefab;
		}
	}
}