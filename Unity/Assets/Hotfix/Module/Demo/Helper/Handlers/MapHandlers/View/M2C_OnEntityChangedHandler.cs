using ETModel;
using Vector3 = UnityEngine.Vector3;

namespace ETHotfix
{
	[MessageHandler]
	public class M2C_OnEntityChangedHandler : AMHandler<M2C_OnEntityChanged>
	{
		protected override async ETTask Run(ETModel.Session session, M2C_OnEntityChanged message)
		{
			Log.Debug($"{message}");

			var entityType = EntityDefine.EntityIds.GetKeyByValue((ushort)message.EntityType);
			ETModel.Entity entity = null;
			if (entityType == typeof(Unit))
			{
				entity = UnitComponent.Instance.Get(message.EntityId);
			}
			if (entityType == typeof(Bullet))
			{
				entity = UnitComponent.Instance.Get(message.EntityId);
			}
			foreach (var item in message.TypeParams)
			{
				var index = message.TypeParams.IndexOf(item);
				var valueString = message.ValueParams.array[index];
				var propertyInfo = EntityDefine.EntityPropertyInfo[(ushort)message.EntityType][(ushort)item];
				if (propertyInfo.PropertyType == typeof(int))
				{
					propertyInfo.SetValue(entity, int.Parse(valueString));
				}
				if (propertyInfo.PropertyType == typeof(float))
				{
					propertyInfo.SetValue(entity, float.Parse(valueString));
				}
				if (propertyInfo.PropertyType == typeof(string))
				{
					propertyInfo.SetValue(entity, valueString);
				}
				if (propertyInfo.PropertyType == typeof(Vector3))
				{
					propertyInfo.SetValue(entity, JsonHelper.FromJson<Vector3>(valueString));
				}
			}
			await ETTask.CompletedTask;
		}
	}
}
