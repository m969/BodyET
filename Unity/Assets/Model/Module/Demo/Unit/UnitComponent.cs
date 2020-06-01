using System.Collections.Generic;
using System.Linq;

namespace ETModel
{
	[ObjectSystem]
	public class UnitComponentSystem : AwakeSystem<UnitComponent>
	{
		public override void Awake(UnitComponent self)
		{
			self.Awake();
		}
	}
	
	public class UnitComponent: EntityComponent<Unit>
	{
		public static UnitComponent Instance { get; private set; }
		public Unit MyUnit;
		

		public override void Awake()
		{
			base.Awake();
			Instance = this;
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			base.Dispose();
			Instance = null;
		}
	}
}