using System;
using ETModel;
using UnityEngine;
using DG.Tweening;

namespace ETHotfix
{
    public static class UnitHotfix
    {
		public static GameObject LocalFire(this Unit self, Vector3 targetPoint, int bulletType, long bulletId)
		{
			var Position = self.Position;
			var BodyView = self.BodyView;

			GizmosDebug.Instance.Path.Clear();
			var p1 = Position + Vector3.up;
			var p2 = targetPoint;

			var bulletColliderObj = self.Fire(targetPoint, bulletType, bulletId);
			bulletColliderObj.name += "|Local";
			var bh = bulletColliderObj.AddComponent<BehaviourAction>();
			bh.OnCollisionEnterAction += (b, collider) => {
				if (collider.gameObject.tag == "LocalPlayer")
					return;
				//Log.Debug($"{collider.gameObject.tag}");
				GameObject.Destroy(bulletColliderObj);
			};
			bh.OnDestroyAction += (b) => {

			};
			//bulletColliderObj.transform.DOMove(p2, 1).onComplete = () => { GameObject.Destroy(bulletColliderObj); };

			//GizmosDebug.Instance.Path.Add(p1);
			//GizmosDebug.Instance.Path.Add(p2);


			//self.ETCancellationTokenSource.Cancel();
			//await TimerComponent.Instance.WaitAsync(1000);
			//if (TimeHelper.ClientNow() - self.LastFireTime >= 1000)
			//	self.ExampleCharacterController.OrientationSharpness = 10;

			//Log.Debug($"{p1} {SkillDiretorTrm.forward}");
			//var ray = new Ray(p1, SkillDiretorTrm.forward);
			//GizmosDebug.Instance.Ray = ray;

			//if (Physics.Raycast(p1, p2, out var hit))//Game.Scene.GetComponent<OperaComponent>().mapMask
			//if (Physics.Raycast(ray, out var hit))//Game.Scene.GetComponent<OperaComponent>().mapMask
			//	{
			//	if (hit.collider.CompareTag("Player"))
			//	{
			//		Log.Debug($"addhp -10");
			//	}
			//}
			return bulletColliderObj;
		}

		public static GameObject Fire(this Unit self, Vector3 targetPoint, int bulletType, long bulletId)
		{
			var Position = self.Position;
			var BodyView = self.BodyView;

			GizmosDebug.Instance.Path.Clear();
			var p1 = Position + Vector3.up;
			var p2 = targetPoint;

			var bulletColliderObj = GameObject.Instantiate(PrefabHelper.GetUnitPrefab("BulletCollider"), p1, Quaternion.identity);
			bulletColliderObj.name = $"Bullet|{bulletType}|{bulletId}";
			var bulletModelObj = GameObject.Instantiate(PrefabHelper.GetUnitPrefab("BulletSmallBlue"), p1, Quaternion.identity);
			bulletModelObj.transform.parent = bulletColliderObj.transform;
			bulletColliderObj.transform.DOMove(p2, 1).onComplete = () => { GameObject.Destroy(bulletColliderObj); };
			return bulletColliderObj;
		}
	}
}