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
        private double _walkSpeed = 20;


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
        
        public double WalkSpeed
        {
            get => _walkSpeed;
            set => _walkSpeed = value;
        }
    }
}