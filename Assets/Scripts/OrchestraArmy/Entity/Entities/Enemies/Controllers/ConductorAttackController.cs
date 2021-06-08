using System.Collections.Generic;
using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Entity.Entities.Projectiles;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Enemies.Controllers
{
    /// <summary>
    /// AttackController for the conductor. Able to fire multiple projectiles at once.
    /// </summary>
    public class ConductorAttackController: IEnemyAttackController
    {
        /// <summary>
        /// The player
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// The attacking enemy
        /// </summary>
        public Enemy Enemy { get; set; }

        /// <summary>
        /// Handle the attack.
        /// </summary>
        public void HandleAttack()
        {
            var state = Enemy.Behaviour.CurrentState.StateData;
            var enemyTransform = Enemy.transform;
            enemyTransform.forward = (Player.transform.position - enemyTransform.position).normalized;

            var angles = new List<float>();
            
            //the amount of notes for 1 - 90 degrees
            var steps = state.ProjectileCount / 2;

            if (steps != 0)
            {
                //the size of one step. Set to 45 if there is only 1
                var step = steps == 1 ? 45 : 90 / steps;
    
                //set the angles while i <= 90 and index < steps
                for (int i = step, index = 0; i <= 90 && index < steps; i += step, index++)
                {
                    angles.Add(i * (Mathf.PI / 180));
                    angles.Add(-i * (Mathf.PI / 180));
                }
            }
            
            //if the amount of projectiles is uneven, we have 1 going straight at the player
            if (state.ProjectileCount % 2 == 1)
                angles.Add(0);

            foreach (var angle in angles)
            {
                var forward = enemyTransform.forward;
                
                //convert the angle to a direction vector
                var directionVector = new Vector3(Mathf.Cos(angle) * forward.x - Mathf.Sin(angle) * forward.z, 0, Mathf.Sin(angle) * forward.x + Mathf.Cos(angle) * forward.z);
                
                //spawn the note
                var obj = (GameObject) Object.Instantiate(
                    Resources.Load($"Prefabs/Projectiles/{state.ProjectileType.Name}"),
                    enemyTransform.position + enemyTransform.forward * (enemyTransform.localScale.x * 1.1f),
                    Enemy.transform.GetChild(0).transform.rotation
                );
                
                EnemyNote attack = (EnemyNote) obj.GetComponent(state.ProjectileType);

                attack.transform.forward = directionVector;
                attack.Source = obj.transform.position;
                attack.MaxDistance = 7.5f;
                attack.Instrument = WeaponFactory.Make(Enemy.WeaponType);
                attack.Attacker = Enemy;
                attack.Tone = state.ProjectileTone;
            }
        }
    }
}