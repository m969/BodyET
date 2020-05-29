using ETModel;
using Vector3 = UnityEngine.Vector3;
using DG.Tweening;

namespace ETHotfix
{
	[MessageHandler]
	public class M2C_OnEntityChangedHandler : AMHandler<M2C_OnEntityChanged>
	{
		protected override async ETTask Run(ETModel.Session session, M2C_OnEntityChanged message)
		{
			var entityType = EntityDefine.TypeIds.GetKeyByValue((ushort)message.EntityType);
			ETModel.Entity entity = null;
			if (entityType == typeof(Unit))
			{
				entity = UnitComponent.Instance.Get(message.EntityId);
			}
			if (entityType == typeof(Bullet))
			{
				var bullet = BulletComponent.Instance.Get(message.EntityId);
				entity = bullet;
				if (bullet.BodyView != null)
					bullet.BodyView.transform.DOMove(new Vector3(message.X / 100f, message.Y / 100f, message.Z / 100f), 0.2f);
			}
			if (entityType == typeof(Monster))
			{
				var monster = MonsterComponent.Instance.Get(message.EntityId);
				entity = monster;
				if (monster.BodyView != null)
					monster.BodyView.transform.DOMove(new Vector3(message.X / 100f, message.Y / 100f, message.Z / 100f), 0.2f);
			}
			if (entity is null)
				return;
			if (message.ComponentType > 0)
				entity = entity.GetComponent(EntityDefine.GetType(message.ComponentType));

			var propertyCollection = EntityDefine.PropertyCollectionMap[EntityDefine.GetTypeId(entity.GetType())];
			if (propertyCollection.ContainsKey((ushort)message.PropertyId))
				entity.SetPropertyValue((ushort)message.PropertyId, message.PropertyValue.bytes);
			await ETTask.CompletedTask;
		}
	}
}
