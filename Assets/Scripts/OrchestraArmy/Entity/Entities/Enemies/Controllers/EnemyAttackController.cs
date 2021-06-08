using System;
using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Entities.Behaviour.Data;
using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Entity.Entities.Projectiles;
using OrchestraArmy.Enum;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace OrchestraArmy.Entity.Entities.Enemies.Controllers
{
    public class EnemyAttackController: IEnemyAttackController
    {
        public Player Player { get; set; }
        public Enemy Enemy { get; set; }
        
        public void HandleAttack()
        {
            Transform enemyTransform;
            Transform playerTransform;
            StateData state = Enemy.Behaviour.CurrentState.StateData;
            
            // Check if enemy and player are still alive
            try
            {
                enemyTransform = Enemy.transform;
                playerTransform = Player.transform;
            }
            catch (Exception e)
            {
                return;
            }

            enemyTransform = Enemy.transform;
            playerTransform = Player.transform;
            Vector3 playerPosition = playerTransform.position;

            Vector3 scale = enemyTransform.localScale;
            scale.y = 0;

            Vector3 enemyPosition = enemyTransform.position;
            enemyPosition.y = 0.5f;
            
            enemyTransform.forward = (playerPosition - enemyPosition).normalized;
            
            var obj = (GameObject) Object.Instantiate(
                Resources.Load($"Prefabs/Projectiles/{state.ProjectileType.Name}"),
                enemyPosition + (enemyTransform.forward * (scale.x * 1.1f)),
                Enemy.transform.GetChild(0).transform.rotation
            );
            
            EnemyNote attack = (EnemyNote) obj.GetComponent(state.ProjectileType);
            
            // Calculate the vector from the note prefab to the player (50% chance on direct shot, 50% chance on predicted shot)
            if (Random.value > 0.5f)
                attack.transform.forward = AimBot(100, Player, enemyPosition, attack.MovementData.WalkSpeed,
                    new Vector3());
            else
                attack.transform.forward = (playerPosition - obj.transform.position).normalized;
            
            // set the attacking source.
            attack.Source = obj.transform.position;
            
            // max distance = 7.5 Unity units
            attack.MaxDistance = 7.5f;
            
            attack.Instrument = WeaponFactory.Make(Enemy.WeaponType);
            attack.Attacker = Enemy;
            attack.Tone = state.ProjectileTone;
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