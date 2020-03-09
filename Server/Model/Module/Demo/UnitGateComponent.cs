namespace ETModel
{
	[ObjectSystem]
	public class UnitGateComponentAwakeSystem : AwakeSystem<UnitGateComponent, long>
	{
		public override void Awake(UnitGateComponent self, long a)
		{
			self.Awake(a);
		}
	}

	public class UnitGateComponent : Entity, ISerializeToEntity
	{
		public long GateSessionActorId { get; set; }

		public bool IsDisconnect { get; set; }

		public void Awake(long gateSessionId)
		{
			this.GateSessionActorId = gateSessionId;
			this.IsDisconnect = false;
		}
	}
}