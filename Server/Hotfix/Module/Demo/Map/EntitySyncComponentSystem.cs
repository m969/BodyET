using System.Collections.Generic;
using System.Threading;
using ETModel;
using PF;
using UnityEngine;

namespace ETHotfix
{
	[ObjectSystem]
	public class EntitySyncComponentAwakeSystem : AwakeSystem<EntitySyncComponent>
	{
		public override void Awake(EntitySyncComponent self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class EntitySyncComponentUpdateSystem : UpdateSystem<EntitySyncComponent>
	{
		public override void Update(EntitySyncComponent self)
		{
			self.Update();
		}
	}

	public static class EntitySyncComponentSystem
	{
		public static void Awake(this EntitySyncComponent self)
		{
			self.Interval = 1000 / self.Fps;
		}

		public static void Update(this EntitySyncComponent self)
		{
			if (TimeHelper.Now() - self.Timer > self.Interval)
			{
				self.Timer = TimeHelper.Now();
				var transform = self.Parent.GetComponent<TransformComponent>();
				var lp = transform.lastPosition;
				var p = transform.position;
				if (Vector3.Distance(lp, p) < 0.1f)
					return;
				transform.lastPosition = p;

				var msg = new M2C_OnEntityChanged();
				msg.EntityId = self.Id;
				msg.EntityType = EntityDefine.GetTypeId(self.Parent.GetType());

				msg.X = (int)(p.x * 100);
				msg.Y = (int)(p.y * 100);
				msg.Z = (int)(p.z * 100);
				MessageHelper.Broadcast(self.Domain, msg);
			}
		}
	}
}