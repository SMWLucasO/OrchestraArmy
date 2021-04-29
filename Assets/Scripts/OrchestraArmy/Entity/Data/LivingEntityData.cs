using UnityEngine;

namespace OrchestraArmy.Entity.Data
{
    
    [System.Serializable]
    public class LivingEntityData
    {
        [Min(0)]
        [SerializeField]
        private int _health = 100;
        
        [Min(0)]
        [SerializeField]
        private int _stamina = 100;
        
        [Min(0)]
        [SerializeField]
        private float _walkSpeed = 200;


        public int Health
        {
            get => _health;
            set => _health = value;
        }

        public int Stamina
        {
            get => _stamina;
            set => _stamina = value;
        }
        
        public float WalkSpeed
        {
            get => _walkSpeed;
            set => _walkSpeed = value;
        }
    }
}