using System.Collections.Generic;
using System.Linq;
using System;

namespace ETModel
{
	public class EntityComponent<T> : Entity where T : Entity
	{
		protected readonly Dictionary<long, T> idEntitys = new Dictionary<long, T>();

		public virtual void Awake()
		{
		}

		public void Add(T entity)
		{
			this.idEntitys.Add(entity.Id, entity);
			entity.Parent = this;
		}

		public T Get(long id)
		{
			T entity;
			this.idEntitys.TryGetValue(id, out entity);
			return entity;
		}

		public void Remove(long id)
		{
			this.idEntitys.Remove(id);
		}

		public int Count
		{
			get
			{
				return this.idEntitys.Count;
			}
		}

		public T[] GetAll()
		{
			return this.idEntitys.Values.ToArray();
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			
			base.Dispose();

			foreach (T entity in this.idEntitys.Values)
			{
				entity.Dispose();
			}
		}
	}
}