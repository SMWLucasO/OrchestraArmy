using OrchestraArmy.Event.Events;

namespace OrchestraArmy.Event.Events.Player
{
    public class PlayerDeathEvent: IEvent
    {
        /// <summary>
        /// The person that died.
        /// </summary>
        public Entity.Entities.Players.Player Player { get; set; }
    }
}