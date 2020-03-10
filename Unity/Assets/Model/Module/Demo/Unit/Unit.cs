using PF;
using UnityEngine;
using System.Threading.Tasks;

namespace ETModel
{

	[ObjectSystem]
	public class UnitUpdateSystem : UpdateSystem<Unit>
	{
		public override void Update(Unit self)
		{
			self.Update();
		}
	}

	[EntityDefine(1)]
	public partial class Unit: Entity
	{
		private string nickname;
		[PropertyDefine(101, SyncFlag.AllClients)]
		public string Nickname { get { return nickname; } set { nickname = value; PublishProperty(nameof(Nickname), value); } }
		
		private int hp;
		public int HP { get { return hp; } set { hp = value; PublishProperty(nameof(HP), value); } }

		public bool Firing { get; set; }

		public TransformComponent Transform { get { return GetComponent<TransformComponent>(); } }


		public void Awake()
		{
			Nickname = "";
			HP = 100;
			Firing = false;
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			base.Dispose();
		}

		public static System.Action<Entity, string, string> OnPropertyChanged { get; set; }
		private void PublishProperty(string name, string value)
		{
			OnPropertyChanged?.Invoke(this, name, value);
		}

		private void PublishProperty(string name, int value)
		{
			PublishProperty(name, $"{value}");
		}

		private void PublishProperty(string name, float value)
		{
			PublishProperty(name, $"{value}");
		}
	}
}