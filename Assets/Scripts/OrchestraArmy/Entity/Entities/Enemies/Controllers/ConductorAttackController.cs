using System.Collections.Generic;
using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Entity.Entities.Projectiles;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Enemies.Controllers
{
    public class ConductorAttackController: IEnemyAttackController
    {
        public Player Player { get; set; }

        public Enemy Enemy { get; set; }
        public void HandleAttack()
        {
            var state = Enemy.Behaviour.CurrentState.StateData;
            var enemyTransform = Enemy.transform;
            enemyTransform.forward = (Player.transform.position - enemyTransform.position).normalized;

            var angles = new List<float>();
            var steps = state.ProjectileCount / 2;
            var step = steps == 1 ? 45 : 90 / steps;

            for (int i = step, index = 0; i <= 90 && index < steps; i += step, index++)
            {
                angles.Add(i * (Mathf.PI / 180));
                angles.Add(-i * (Mathf.PI / 180));
            }
            
            if (state.ProjectileCount % 2 == 1)
                angles.Add(0);

            foreach (var angle in angles)
            {
                var forward = enemyTransform.forward;
                var directionVector = new Vector3(Mathf.Cos(angle) * forward.x - Mathf.Sin(angle) * forward.z, 0, Mathf.Sin(angle) * forward.x + Mathf.Cos(angle) * forward.z);
                
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