using System;
using OrchestraArmy.Entity.Entities.Behaviour.Data;
using OrchestraArmy.Entity.Entities.Behaviour.Utils;
using OrchestraArmy.Entity.Entities.Enemies;
using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Entity.Entities.Projectiles;
using OrchestraArmy.Enum;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;
using OrchestraArmy.Event.Events.Player;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace OrchestraArmy.Entity.Entities.Behaviour
{
    public class AttackBehaviour : IBehaviourState, IListener<EnemyTurnEvent>
    {
        /// <summary>
        /// Data used by the state.
        /// </summary>
        public StateData StateData { get; set; }

        /// <summary>
        /// Enter this state.
        /// </summary>
        public void Enter()
        {
            EventManager.Bind<EnemyTurnEvent>(this);
            EventManager.Invoke(new CombatInitiatedEvent() {EntityId = StateData.Enemy.GetInstanceID()});
        }

        /// <summary>
        /// Process this state.
        /// </summary>
        public void Process(BehaviourStateMachine machine)
        {
            Transform enemyTransform = StateData.Enemy.transform;
            Transform playerTransform = StateData.Player.transform;
         
            Vector3 scale = enemyTransform.localScale;
            scale.y = 0;

            float distance = BehaviourUtil.GetScaleInclusiveDistance(StateData.Player, StateData.Enemy);
            
            // If the player can't be seen: go to the MoveToPlayer behaviour.
            if (!BehaviourUtil.EnemyCanDetectPlayer(StateData.Player, StateData.Enemy, 8 + scale.x))
            {
                machine.SetState(new MoveToPlayerBehaviour());
                return;
            }
            
            // Movement for combat
            Vector3 playerPosition = playerTransform.position;
            Vector3 playerRelativePosition = enemyTransform.InverseTransformPoint(playerPosition);
            NavMeshAgent navAgent = StateData.Enemy.NavMeshAgent;

            float destDist = 4.0f;
            try
            {
                destDist = (navAgent.destination - playerPosition).magnitude;
            } catch(Exception){}
            
            // Prevent destination outside attack range
            if (destDist<2.5 || destDist>7.5)
            {
                navAgent.isStopped = true;
                navAgent.ResetPath();
            }   
            
            // Avoid to close combat SHOULD NEVER HAPPEN if player is slower than enemy
            if (distance < 2)   
            {
                // Set destination in opposite direction of player to optimal attack range (4 units) 
                navAgent.SetDestination((-playerRelativePosition.normalized * (5 - distance)) + enemyTransform.position);
                return;
            }

            if ((navAgent.destination - enemyTransform.position).magnitude <0.1f) //prevent jittering movement
            {
                // Do random movement but keep player inside attack area
                Vector3 movement = Random.insideUnitSphere * 1.249f; // <2.5 units diameter circle
                movement.y = 0;
    
                // Shift so player always in attack area
                movement += (playerRelativePosition - playerRelativePosition.normalized * 5.0f); 
                
                // Move to point movement  (movement is position relative to enemy)
                navAgent.SetDestination(movement + enemyTransform.position);  
            }
            
            
        }

        /// <summary>
        /// Exit this state.
        /// </summary>
        public void Exit()
        {
            EventManager.Unbind<EnemyTurnEvent>(this);
            EventManager.Invoke(new LeaveCombatEvent() {EntityId = StateData.Enemy.GetInstanceID()});
        }

        public void OnEvent(EnemyTurnEvent invokedEvent)
        {
            Transform enemyTransform;
            Transform playerTransform;
            
            //check if enemy and player are still allive
            try
            {
                enemyTransform = StateData.Enemy.transform;
                playerTransform = StateData.Player.transform;
            }
            catch (Exception)
            {
                return;
            }

            enemyTransform = StateData.Enemy.transform;
            playerTransform = StateData.Player.transform;
            Vector3 playerPosition = playerTransform.position;

            Vector3 scale = enemyTransform.localScale;
            scale.y = 0;

            Vector3 enemyPosition = enemyTransform.position;
            enemyPosition.y = 0.5f;
            
            enemyTransform.forward = (playerPosition - enemyPosition).normalized;
            
            // generate the enemy note to be shot.
            var obj = (GameObject) Object.Instantiate(
                Resources.Load("Prefabs/EnemyNoteProjectile"),
                enemyPosition + (enemyTransform.forward * (scale.x * 1.1f)),
                StateData.Enemy.transform.GetChild(0).transform.rotation
            );
            
            var attack = obj.GetComponent<EnemyNote>();
            
            // calculate the vector from the note prefab to the player (50% chance on direct shot, 50% chance on predicted shot)
            if (Random.value > 0.5f)
                attack.transform.forward = AimBot(100, StateData.Player, enemyPosition, attack.MovementData.WalkSpeed,
                    new Vector3());
            else
                attack.transform.forward = (playerPosition - obj.transform.position).normalized;
            
            // set the attacking source.
            attack.Source = obj.transform.position;
            
            // max distance = 7.5 Unity units
            attack.MaxDistance = 7.5f;
            
            attack.Instrument = WeaponFactory.Make(StateData.Enemy.WeaponType);
            attack.Attacker = StateData.Enemy;
            attack.Tone = Tone.A; // TODO: determine this another way.

            if (invokedEvent.EnemyId != StateData.Enemy.GetInstanceID())
                return;
            
            //only fire the attack event when it is the relevant enemies turn
            EventManager.Invoke(new EnemyAttackEvent()
            {
                Tone = attack.Tone,
                Instrument = StateData.Enemy.WeaponType,
                Position = StateData.Enemy.transform.position
            });
        }
        
        /// <summary>
        /// calculates the vector of the note with player movement
        /// </summary>
        /// <param name="depth">acuracy of the aiming</param>
        /// <param name="player">player entity to shoot</param>
        /// <param name="enemyPosition">location of the enemy</param>
        /// <param name="bulletSpeed"></param>
        /// <param name="dirGuess">guess location interception</param>
        /// <param name="timeGuess">guess time interception</param>
        /// <returns></returns>
        private Vector3 AimBot(int depth, Player player, Vector3 enemyPosition, float bulletSpeed, Vector3 dirGuess,float timeGuess = -1.0f)
        {
            if (timeGuess == -1.0f)
                timeGuess = (player.transform.position - enemyPosition).magnitude / bulletSpeed;
            else
                timeGuess = (dirGuess - enemyPosition).magnitude / bulletSpeed;
            dirGuess = (player.transform.position - enemyPosition) + (player.transform.forward * (player.RigidBody.velocity.magnitude * timeGuess));
            if (depth > 0)
                return(AimBot(depth-1, player, enemyPosition,bulletSpeed, dirGuess, timeGuess));
            return(dirGuess.normalized);
        }
    }
}