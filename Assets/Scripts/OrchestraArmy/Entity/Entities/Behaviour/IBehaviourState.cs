using OrchestraArmy.Entity.Entities.Behaviour.Data;

namespace OrchestraArmy.Entity.Entities.Behaviour
{
    public interface IBehaviourState
    {
        /// <summary>
        /// Data used by the state.
        /// </summary>
        public StateData StateData { get; set; }

        /// <summary>
        /// Enter this state.
        /// </summary>
        public void Enter();
        
        /// <summary>
        /// Process this state.
        /// </summary>
        public void Process(BehaviourStateMachine machine);
        
        /// <summary>
        /// Exit this state.
        /// </summary>
        public void Exit();

    }
}