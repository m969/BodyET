using System.Collections.Generic;
using System.Linq;

namespace ETModel
{
	[ObjectSystem]
	public class MonsterComponentSystem : AwakeSystem<MonsterComponent>
	{
		public override void Awake(MonsterComponent self)
		{
			self.Awake();
		}
	}
	
	public class MonsterComponent : EntityComponent<Monster>
	{
		public static MonsterComponent Instance { get; private set; }

		public override void Awake()
		{
			base.Awake();
			Instance = this;
		}

		public override void Dispose()
		{
			base.Dispose();
			Instance = null;
		}
	}
}