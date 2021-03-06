using OrchestraArmy.Entity.Entities.Behaviour.Data;
using OrchestraArmy.Entity.Entities.Behaviour.Utils;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace OrchestraArmy.Entity.Entities.Behaviour
{
    public class MoveToPlayerBehaviour : IBehaviourState
    {
        public StateData StateData { get; set; }

        public void Enter() { }

        public void Process(BehaviourStateMachine machine)
        {
            Transform enemyTransform = StateData.Enemy.transform;
            
            Vector3 scale = enemyTransform.localScale;
            scale.y = 0;

            float distance = BehaviourUtil.GetScaleInclusiveDistance(StateData.Player, StateData.Enemy);
            
            // When the player is more than 12 units away from the enemy: wander.
            if (distance >= 12)
            {
                machine.SetState(new WanderBehaviour());
                return;
            }
            
            // When the enemy is within 7 units of the player & it can detect the player: attack.
            if (BehaviourUtil.EnemyCanDetectPlayer(StateData.Player, StateData.Enemy, 7f + scale.x))
            {
                machine.SetState(new AttackBehaviour());
                return;
            }

            // When the player can't be seen, but the player is in range: just stand still.
            if (!BehaviourUtil.EnemyCanDetectPlayer(StateData.Player, StateData.Enemy, 5 + scale.x))
                return;
            
            // Move the enemy towards the player.
            Vector3 offset = (StateData.Enemy.transform.forward.normalized * (2.9f + scale.x));
            Vector3 enemyDestination = StateData.Player.transform.position - offset;
                
            StateData.Enemy.NavMeshAgent.SetDestination(
                enemyDestination
            );

        }

        public void Exit() { }
    }
}