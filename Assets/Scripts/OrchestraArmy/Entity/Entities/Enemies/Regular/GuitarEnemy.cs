using OrchestraArmy.Entity.Entities.Enemies.Data;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace OrchestraArmy.Entity.Entities.Enemies.Regular
{
    public class GuitarEnemy : Enemy
    {

        public override WeaponType HittableBy { get; set; }
            = WeaponType.Guitar;
        
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