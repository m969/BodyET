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
			var entityType = EntityDefine.EntityIds.GetKeyByValue((ushort)message.EntityType);
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
			if (entity is null)
				return;

			var propertyCollection = EntityDefine.PropertyCollectionMap[EntityDefine.GetTypeId(entity.GetType())];
			if (propertyCollection.ContainsKey((ushort)message.PropertyId))
				entity.SetPropertyValue((ushort)message.PropertyId, message.PropertyValue.bytes);

			//var valueString = message.PropertyValue.bytes;
			//var propertyCollection = EntityDefine.PropertyCollectionMap[(ushort)message.EntityType];
			//var propertyInfo = propertyCollection[(ushort)message.PropertyId];
			//if (propertyInfo.PropertyType == typeof(int))
			//{
			//	//propertyInfo.SetValue(entity, MongoHelper.ToInt(valueString));
			//	typeof(ReactProperty<int>).GetProperty("Value").SetValue(propertyInfo.GetValue(entity), MongoHelper.ToInt(valueString));
			//}
			//if (propertyInfo.PropertyType == typeof(float))
			//{
			//	//propertyInfo.SetValue(entity, MongoHelper.ToFloat(valueString));
			//	typeof(ReactProperty<float>).GetProperty("Value").SetValue(propertyInfo.GetValue(entity), MongoHelper.ToFloat(valueString));
			//}
			//if (propertyInfo.PropertyType == typeof(string))
			//{
			//	//propertyInfo.SetValue(entity, MongoHelper.FromBson<string>(valueString));
			//	typeof(ReactProperty<string>).GetProperty("Value").SetValue(propertyInfo.GetValue(entity), MongoHelper.FromBson<string>(valueString));
			//}
			//if (propertyInfo.PropertyType == typeof(Vector3))
			//{
			//	//propertyInfo.SetValue(entity, MongoHelper.FromBson<Vector3>(valueString));
			//	typeof(ReactProperty<Vector3>).GetProperty("Value").SetValue(propertyInfo.GetValue(entity), MongoHelper.FromBson<Vector3>(valueString));
			//}

			await ETTask.CompletedTask;
		}
	}
}
