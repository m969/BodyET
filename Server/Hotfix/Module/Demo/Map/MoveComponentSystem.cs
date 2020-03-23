using System.Collections.Generic;
using System.Threading;
using ETModel;
using PF;
using UnityEngine;

namespace ETHotfix
{
	[ObjectSystem]
	public class MoveComponentAwakeSystem : AwakeSystem<MoveComponent>
	{
		public override void Awake(MoveComponent self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class MoveComponentUpdateSystem : UpdateSystem<MoveComponent>
	{
		public override void Update(MoveComponent self)
		{
			self.Update();
		}
	}

	public static class MoveComponentSystem
	{
		public static void Awake(this MoveComponent self)
		{
			
		}

		public static void Update(this MoveComponent self)
		{

		}
	}
}