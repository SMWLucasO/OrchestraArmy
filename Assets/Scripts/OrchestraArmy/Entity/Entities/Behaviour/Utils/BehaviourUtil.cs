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

        /// <summary>
        /// Get the distance between the player and an enemy, taking into account the scale of the enemy.
        /// Assumes that the scaling in X, Y and Z are the same value.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="enemy"></param>
        /// <returns>The distance with scale taken into account.</returns>
        public static float GetScaleInclusiveDistance(Player player, Enemy enemy) 
            => Vector3.Distance(player.transform.position, enemy.transform.position) - enemy.transform.localScale.x;
    }
}