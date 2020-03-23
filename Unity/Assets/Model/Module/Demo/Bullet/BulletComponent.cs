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
	
	public class BulletComponent : Entity
	{
		public static BulletComponent Instance { get; private set; }
		private readonly Dictionary<long, Bullet> idBullets = new Dictionary<long, Bullet>();

		public void Awake()
		{
			Instance = this;
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			base.Dispose();

			foreach (Bullet Bullet in this.idBullets.Values)
			{
				Bullet.Dispose();
			}

			this.idBullets.Clear();

			Instance = null;
		}

		public void Add(Bullet Bullet)
		{
			this.idBullets.Add(Bullet.Id, Bullet);
			Bullet.Parent = this;
		}

		public Bullet Get(long id)
		{
			Bullet Bullet;
			this.idBullets.TryGetValue(id, out Bullet);
			return Bullet;
		}

		public void Remove(long id)
		{
			Bullet Bullet;
			this.idBullets.TryGetValue(id, out Bullet);
			this.idBullets.Remove(id);
			Bullet?.Dispose();
		}

		public void RemoveNoDispose(long id)
		{
			this.idBullets.Remove(id);
		}

		public int Count
		{
			get
			{
				return this.idBullets.Count;
			}
		}

		public Bullet[] GetAll()
		{
			return this.idBullets.Values.ToArray();
		}
	}
}