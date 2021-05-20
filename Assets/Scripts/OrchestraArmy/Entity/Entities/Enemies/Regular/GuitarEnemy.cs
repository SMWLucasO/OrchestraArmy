using OrchestraArmy.Entity.Entities.Enemies.Data;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace OrchestraArmy.Entity.Entities.Enemies.Regular
{
    public class GuitarEnemy : Enemy
    {
        private NavMeshAgent _navMeshAgent;

        void Start()
        {
            _navMeshAgent = this.GetComponent<NavMeshAgent>();
        }
        public StateData StateData { get; set; }
        void update()
        {
            if (Keyboard.current.nKey.isPressed)
            {
                Vector3 dest = StateData.Player.transform.position;
                _navMeshAgent.SetDestination(dest);
            }
        }
    }
}