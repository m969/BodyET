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
			if (message.PropertyId == 0)
				return;
			{
				//var index = message.TypeParams.IndexOf(item);
				//var valueString = message.ValueParams.array[index];
				var valueString = message.PropertyValue.bytes;
				var propertyInfo = EntityDefine.EntityPropertyInfo[(ushort)message.EntityType][(ushort)message.PropertyId];

				//switch (propertyInfo.PropertyType)
				//{
				//	case typeof(int):
				//		break;
				//	default:
				//		break;
				//}

				if (propertyInfo.PropertyType == typeof(int))
				{
					propertyInfo.SetValue(entity, MongoHelper.ToInt(valueString));
					//propertyInfo.SetValue(entity, int.Parse(valueString));
				}
				if (propertyInfo.PropertyType == typeof(float))
				{
					propertyInfo.SetValue(entity, MongoHelper.ToFloat(valueString));
					//propertyInfo.SetValue(entity, float.Parse(valueString));
				}
				if (propertyInfo.PropertyType == typeof(string))
				{
					propertyInfo.SetValue(entity, MongoHelper.FromBson<string>(valueString));
					//propertyInfo.SetValue(entity, valueString);
				}
				if (propertyInfo.PropertyType == typeof(Vector3))
				{
					propertyInfo.SetValue(entity, MongoHelper.FromBson<Vector3>(valueString));
					//propertyInfo.SetValue(entity, JsonHelper.FromJson<Vector3>(valueString));
				}
			}
			await ETTask.CompletedTask;
		}
	}
}
