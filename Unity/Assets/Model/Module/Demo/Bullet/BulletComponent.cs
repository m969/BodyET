using System.Collections.Generic;
using System.Linq;

namespace ETModel
{
	[ObjectSystem]
	public class BulletComponentSystem : AwakeSystem<BulletComponent>
	{
		public override void Awake(BulletComponent self)
		{
			self.Awake();
		}
	}
	
	public class BulletComponent : EntityComponent<Bullet>
	{
		public static BulletComponent Instance { get; private set; }

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