using OrchestraArmy.Entity.Entities.Enemies;
using OrchestraArmy.Entity.Entities.Players;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Behaviour.Utils
{
    public static class BehaviourUtil
    {
        
        /// <summary>
        /// Detect whether the enemy can see the player, given a specified detection range.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="enemy"></param>
        /// <param name="detectionRange"></param>
        /// <returns></returns>
        public static bool EnemyCanDetectPlayer(Player player, Enemy enemy, float detectionRange = 5)
        {
            // try to see the player
            // angle to the player
            Vector3 enemyPosition = enemy.transform.position;
            
            Vector3 directionTowardsPlayer = (player.transform.position - enemyPosition).normalized;
            
            Ray r = new Ray(enemyPosition, directionTowardsPlayer);

            Physics.Raycast(r, out RaycastHit hitEntity, detectionRange);

            if (hitEntity.transform == null)
                return false;
            
            return hitEntity.transform.CompareTag("Player");
        }

    }
}