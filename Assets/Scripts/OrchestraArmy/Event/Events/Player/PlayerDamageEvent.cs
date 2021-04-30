using OrchestraArmy.Event.Events;

namespace OrchestraArmy.Event.Events.Player
{
    public class PlayerDamageEvent: IEvent
    {
        public int HealthLost { get; set; }
    }
}