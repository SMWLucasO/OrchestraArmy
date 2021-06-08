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
            
            int angleDelta = 180/(state.ProjectileCount+1);
            
            for (int i = angleDelta; i<=180 ;i += angleDelta)
            {
                var angle = i - 90;
                var directionVector = new Vector3(-Mathf.Cos(angle), 0, -Mathf.Sin(angle));
                
                var obj = (GameObject) Object.Instantiate(
                    Resources.Load($"Prefabs/Projectiles/{state.ProjectileType.Name}"),
                    enemyTransform.position + enemyTransform.forward * (enemyTransform.localScale.x * 1.1f) + directionVector,
                    Enemy.transform.GetChild(0).transform.rotation
                );
                
                EnemyNote attack = (EnemyNote) obj.GetComponent(state.ProjectileType);
                
                attack.Source = obj.transform.position;
                attack.MaxDistance = 7.5f;
                attack.Instrument = WeaponFactory.Make(Enemy.WeaponType);
                attack.Attacker = Enemy;
                attack.Tone = state.ProjectileTone;
            }
        }
    }
}