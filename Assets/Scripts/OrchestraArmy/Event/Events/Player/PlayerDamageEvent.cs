namespace OrchestraArmy.Event.Event
{
    public class PlayerDamageEvent: IEvent
    {
        public int HealthLost { get; set; }
    }
}