namespace OrchestraArmy.Event.Events.Enemy
{
    public class CombatInitiatedEvent : IEvent
    {
        public Entity.Entities.Enemies.Enemy Entity;
    }
}