using System.Collections.Generic;
using System.Linq;

namespace ETModel
{
	[ObjectSystem]
	public class PlayerComponentAwakeSystem : AwakeSystem<PlayerComponent>
	{
		public override void Awake(PlayerComponent self)
		{
			self.Awake();
		}
	}
	
	public class PlayerComponent : EntityComponent<Player>
	{
		public static PlayerComponent Instance { get; private set; }

		private Player myPlayer;

		public Player MyPlayer
		{
			get
			{
				return this.myPlayer;
			}
			set
			{
				this.myPlayer = value;
				this.myPlayer.Parent = this;
			}
		}
		
		public override void Awake()
		{
			Instance = this;
		}

		public override void Dispose()
		{
			base.Dispose();
			Instance = null;
		}
	}
}