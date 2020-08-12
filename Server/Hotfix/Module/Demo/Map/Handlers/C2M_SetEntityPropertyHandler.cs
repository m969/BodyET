//using ETModel;
//using PF;
//using UnityEngine;

//namespace ETHotfix
//{
//	[ActorMessageHandler]
//	public class C2M_SetEntityPropertyHandler : AMActorLocationHandler<Unit, C2M_SetEntityProperty>
//	{
//		protected override async ETTask Run(Unit unit, C2M_SetEntityProperty message)
//		{
//			Log.Msg(message);
//			var propertyCollection = EntityDefine.PropertyCollectionMap[EntityDefine.GetTypeId<Unit>()];
//			if (propertyCollection.ContainsKey((ushort)message.PropertyId))
//				unit.SetPropertyValue((ushort)message.PropertyId, message.PropertyValue.bytes);
//			await ETTask.CompletedTask;
//		}
//	}
//}