using UnityEngine;

namespace ETModel
{
	[EntityDefine(2)]
	public sealed class Bullet : Entity
	{
		[PropertyDefine(201, PropertyType.String, SyncFlag.AllClients)]
		public Vector3 Position { get; set; }

		public void Awake()
		{
			
		}
	}
}