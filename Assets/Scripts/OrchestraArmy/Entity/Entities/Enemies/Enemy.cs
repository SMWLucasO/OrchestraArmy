namespace OrchestraArmy.Entity.Entities.Enemies
{
    public class Enemy : LivingDirectionalEntity
    {
        public BehaviourStateMachine Behaviour { get; set; }
    }
}