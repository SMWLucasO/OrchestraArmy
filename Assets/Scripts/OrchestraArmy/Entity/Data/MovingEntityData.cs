using UnityEngine;

namespace OrchestraArmy.Entity.Data
{
    [System.Serializable]
    public class MovingEntityData
    {
        [Min(0)]
        [SerializeField]
        private float _walkSpeed = 200;

        public float WalkSpeed
        {
            get => _walkSpeed;
            set => _walkSpeed = value;
        }
    }
}