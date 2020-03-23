using PF;
using UnityEngine;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
	[EntityDefine(1)]
	public partial class Unit: Entity
	{
		private string nickname = "";
		[PropertyDefine(101, SyncFlag.AllClients)]
		public string Nickname { get { return nickname; } set { nickname = value; PublishProperty(nameof(Nickname), value); } }

		private int hp;
		[PropertyDefine(102, SyncFlag.AllClients)]
		public int HP { get { return hp; } set { hp = value; PublishProperty(nameof(HP), value); } }
		private int state;
		[PropertyDefine(103, SyncFlag.AllClients)]
		public int State { get { return state; } set { state = value; PublishProperty(nameof(State), value); } }

		private float[] posArr = new float[] { 0, 0, 0 };
		public float[] PosArr
		{
			get
			{
				if (Transform != null)
				{
					posArr[0] = Transform.Position.x;
					posArr[1] = Transform.Position.y;
					posArr[2] = Transform.Position.z;
				}

				return posArr;
			}
			set
			{
				if (Transform != null)
				{
					Transform.Position = new Vector3(value[0], value[1], value[2]);
					posArr[0] = Transform.Position.x;
					posArr[1] = Transform.Position.y;
					posArr[2] = Transform.Position.z;
				}
			}
		}

		public bool Firing { get; set; }
		[BsonIgnore]
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
	}
}