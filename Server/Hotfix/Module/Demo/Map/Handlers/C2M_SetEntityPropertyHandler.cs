using ETModel;
using PF;
using UnityEngine;

namespace ETHotfix
{
	[ActorMessageHandler]
	public class C2M_SetEntityPropertyHandler : AMActorLocationHandler<Unit, C2M_SetEntityProperty>
	{
		protected override async ETTask Run(Unit unit, C2M_SetEntityProperty message)
		{
			Log.Msg(message);

			var propertyCollection = EntityDefine.PropertyCollectionMap[EntityDefine.GetTypeId<Unit>()];
			if (propertyCollection.ContainsKey((ushort)message.PropertyId))
				unit.SetPropertyValue((ushort)message.PropertyId, message.PropertyValue.bytes);

			//var valueString = message.PropertyValue.bytes;
			//var propertyCollection = EntityDefine.PropertyCollectionMap[EntityDefine.GetTypeId<Unit>()];
			//if (propertyCollection.TryGetValue((ushort)message.PropertyId, out var propertyInfo))
			//{
			//	//var propertyInfo = propertyCollection[(ushort)message.PropertyId];
			//	if (propertyInfo.PropertyType == typeof(int))
			//		propertyInfo.SetValue(unit, MongoHelper.ToInt(valueString));
			//	if (propertyInfo.PropertyType == typeof(float))
			//		propertyInfo.SetValue(unit, MongoHelper.ToFloat(valueString));
			//	if (propertyInfo.PropertyType == typeof(string))
			//		propertyInfo.SetValue(unit, MongoHelper.FromBson<string>(valueString));
			//	if (propertyInfo.PropertyType == typeof(Vector3))
			//		propertyInfo.SetValue(unit, MongoHelper.FromBson<Vector3>(valueString));
			//}

			await ETTask.CompletedTask;
		}
	}
}