using System;
using System.Numerics;
using OrchestraArmy.Entity.Entities.Behaviour.Data;
using OrchestraArmy.Entity.Entities.Behaviour.Utils;
using Vector3 = UnityEngine.Vector3;

namespace OrchestraArmy.Entity.Entities.Behaviour
{
    public class MoveToPlayerBehaviour : IBehaviourState
    {
        public StateData StateData { get; set; }

        public void Enter() { }

        public void Process(BehaviourStateMachine machine)
        {
            Vector3 playerPosition = StateData.Player.transform.position;
            Vector3 enemyPosition = StateData.Enemy.transform.position;

            // When the player is more than 5 units away from the enemy: wander.
            if (Vector3.Distance(enemyPosition, playerPosition) > 5)
            {
                machine.SetState(new WanderBehaviour());
                return;
            }
            
            // When the enemy is within 3 units of the player & it can detect the player: attack.
            if (BehaviourUtil.EnemyCanDetectPlayer(StateData.Player, StateData.Enemy, 3))
            {
                machine.SetState(new AttackBehaviour());
                return;
            }

            if (!BehaviourUtil.EnemyCanDetectPlayer(StateData.Player, StateData.Enemy)) return;
            
            Vector3 offset = (StateData.Enemy.transform.forward.normalized * 2.9f);
            Vector3 enemyDestination = StateData.Player.transform.position - offset;
                
            StateData.Enemy.NavMeshAgent.SetDestination(
                enemyDestination
            );

        }

        public void Exit() { }
    }
}