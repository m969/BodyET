using System.Collections.Generic;
using System.Threading;
using ETModel;
using PF;
using UnityEngine;

namespace ETHotfix
{
	[ObjectSystem]
	public class MonsterAwakeSystem : AwakeSystem<Monster>
	{
		public override void Awake(Monster self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class MonsterUpdateSystem : UpdateSystem<Monster>
	{
		public override void Update(Monster self)
		{
			self.Update();
		}
	}

	[ObjectSystem]
	public class MonsterDestroySystem : DestroySystem<Monster>
	{
		public override void Destroy(Monster self)
		{
			self.Destroy();
		}
	}

	public static class MonsterHotfix
    {
		public static void Update(this Monster self)
		{
			if (TimeHelper.Now() - self.Timer > self.Interval)
			{
				self.Timer = TimeHelper.Now();

				var lp = self.Transform.LastPosition;
				var p = self.Transform.Position;
				self.GetComponent<MoveComponent>().MoveTo(p + Vector3.forward * 2).Coroutine();
			}
		}

		public static void Destroy(this Monster self)
		{
			MessageHelper.Broadcast(self.Domain, new M2C_OnLeaveView() { LeaveEntity = self.Id, EntityType = EntityDefine.GetTypeId(self.GetType()) });
		}
	}
}