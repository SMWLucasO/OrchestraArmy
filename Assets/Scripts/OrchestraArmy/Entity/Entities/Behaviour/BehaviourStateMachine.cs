using OrchestraArmy.Entity.Entities.Enemies;

namespace OrchestraArmy.Entity.Entities.Behaviour
{
    public class BehaviourStateMachine
    {
        
        /// <summary>
        /// The owner of this state machine.
        /// </summary>
        public Enemy Owner { get; set; }
        
        /// <summary>
        /// The previous state of the state machine.
        /// </summary>
        public IBehaviourState PreviousState { get; set; }
        
        /// <summary>
        /// The current state of the state machine.
        /// </summary>
        public IBehaviourState CurrentState { get; set; }

        /// <summary>
        /// Set the new state of the state machine.
        /// </summary>
        /// <param name="newState"></param>
        public void SetState(IBehaviourState newState)
        {
            if (newState == null)
                return;
            
            CurrentState.Exit();
            
            // Transfer StateData over to new state.
            newState.StateData = CurrentState.StateData;
            
            newState.Enter();

            this.PreviousState = CurrentState;
            this.CurrentState = newState;
        }

        /// <summary>
        /// Process the current state of the state machine.
        /// </summary>
        public void Update() 
            => CurrentState.Process(this);

        public void ClearState()
        {
            CurrentState.Exit();
            this.PreviousState = CurrentState;
            this.CurrentState = null;
        }
    }
}