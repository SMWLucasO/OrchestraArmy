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
        private int _maxHealth = 100;

        [Min(0)]
        [SerializeField]
        private int _maxStamina = 100;
        
        /// <summary>
        /// The entity's current health.
        /// </summary>
        public int Health
        {
            get => _health;
            set => _health = value;
        }

        /// <summary>
        /// The entity's current stamina.
        /// </summary>
        public int Stamina
        {
            get => _stamina;
            set => _stamina = value;
        }

        /// <summary>
        /// The entity's maximum health
        /// </summary>
        public int MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = value;
        }

        /// <summary>
        /// The entity's maximum stamina
        /// </summary>
        public int MaxStamina
        {
            get => _maxStamina;
            set => _maxStamina = value;
        }
    }
}