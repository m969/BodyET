using ETModel;

namespace ETHotfix
{
    public static class MessageHelper
    {
        public static void OnPropertyChanged(Entity entity, string propertyName, /*string value,*/ byte[] valueBytes)
        {
            var entityType = EntityDefine.EntityIds.GetValueByKey(entity.GetType());
            if (!EntityDefine.PropertyDefineCollectionMap.ContainsKey(entityType))
                return;
            if (!EntityDefine.PropertyDefineCollectionMap[entityType].ContainsKey(propertyName))
                return;

            var attr = EntityDefine.PropertyDefineCollectionMap[entityType][propertyName];
            var msg = new M2C_OnEntityChanged();
            msg.EntityId = entity.Id;
            msg.EntityType = entityType;
            msg.PropertyId = attr.Id;
            msg.PropertyValue.bytes = valueBytes;

            if (attr.Flag == SyncFlag.AllClients)
            {
                Broadcast(entity.Domain, msg);
            }
            if (attr.Flag == SyncFlag.OtherClients)
            {
                if (entity is Unit u)
                {
                    BroadcastToOther(u, msg);
                }
            }
            if (attr.Flag == SyncFlag.OwnClient)
            {
                if (entity is Unit u)
                {
                    Send(u, msg);
                }
            }
        }

        public static void Broadcast(Unit unit, IActorMessage message)
        {
            Broadcast(unit.Domain, message);
        }

        public static void Broadcast(Entity domain, IActorMessage message)
        {
            var units = domain.GetComponent<UnitComponent>().GetAll();

            if (units == null) return;

            foreach (Unit u in units)
            {
                UnitGateComponent unitGateComponent = u.GetComponent<UnitGateComponent>();
                if (unitGateComponent.IsDisconnect)
                {
                    continue;
                }
                SendActor(unitGateComponent.GateSessionActorId, message);
            }
        }

        public static void BroadcastToOther(Unit unit, IActorMessage message)
        {
            var units = unit.Domain.GetComponent<UnitComponent>().GetAll();

            if (units.Length == 0) return;

            foreach (Unit u in units)
            {
                UnitGateComponent unitGateComponent = u.GetComponent<UnitGateComponent>();
                if (unitGateComponent.IsDisconnect)
                    continue;
                if (u == unit)
                    continue;
                SendActor(unitGateComponent.GateSessionActorId, message);
            }
        }

        /// <summary>
        /// 发送协议给ActorLocation
        /// </summary>
        /// <param name="id">注册Actor的Id</param>
        /// <param name="message"></param>
        public static void SendToLocationActor(long id, IActorLocationMessage message)
        {
            ActorLocationSenderComponent.Instance.Send(id, message);
        }
        
        /// <summary>
        /// 发送协议给Actor
        /// </summary>
        /// <param name="actorId">注册Actor的InstanceId</param>
        /// <param name="message"></param>
        public static void Send(Unit u, IActorMessage message)
        {
            UnitGateComponent unitGateComponent = u.GetComponent<UnitGateComponent>();
            if (unitGateComponent.IsDisconnect)
                return;
            ActorMessageSenderComponent.Instance.Send(unitGateComponent.GateSessionActorId, message);
        }

        public static void SendActor(long actorId, IActorMessage message)
        {
            ActorMessageSenderComponent.Instance.Send(actorId, message);
        }

        /// <summary>
        /// 发送RPC协议给Actor
        /// </summary>
        /// <param name="actorId">注册Actor的InstanceId</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async ETTask<IActorResponse> CallActor(long actorId, IActorRequest message)
        {
            return await ActorMessageSenderComponent.Instance.Call(actorId, message);
        }
        
        /// <summary>
        /// 发送RPC协议给ActorLocation
        /// </summary>
        /// <param name="id">注册Actor的Id</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async ETTask<IActorResponse> CallLocationActor(long id, IActorLocationRequest message)
        {
            return await ActorLocationSenderComponent.Instance.Call(id, message);
        }
    }
}