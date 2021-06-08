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
        private int _maxHealth = 100;
        
        [Min(0)]
        [SerializeField]
        private int _stamina = 100;

        [Min(0)]
        [SerializeField]
        private int _maxStamina = 100;

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

        public int MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = value;
        }

        public int MaxStamina
        {
            get => _maxStamina;
            set => _maxStamina = value;
        }
    }
}