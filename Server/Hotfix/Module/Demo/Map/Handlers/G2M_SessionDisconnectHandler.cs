using ETModel;

namespace ETHotfix
{
	[ActorMessageHandler]
	public class G2M_SessionDisconnectHandler : AMActorLocationHandler<Unit, G2M_SessionDisconnect>
	{
		protected override async ETTask Run(Unit unit, G2M_SessionDisconnect message)
		{
			unit.GetComponent<UnitGateComponent>().IsDisconnect = true;
			await DBComponent.Instance.Save(unit);
			PlayerComponent.Instance.Remove(unit.PlayerId);
			unit.Domain.GetComponent<UnitComponent>().Remove(unit.Id);
			await ETTask.CompletedTask;
		}
	}
}