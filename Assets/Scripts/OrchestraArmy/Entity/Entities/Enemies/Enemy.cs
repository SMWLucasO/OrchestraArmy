namespace OrchestraArmy.Entity.Entities.Enemies
{
    public abstract class Enemy : LivingDirectionalEntity
    {
        public BehaviourStateMachine Behaviour { get; set; }
    }
}