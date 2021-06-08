using OrchestraArmy.Entity.Entities.Enemies.Controllers;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Entity.Entities.Projectiles;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OrchestraArmy.Entity.Entities.Enemies.Bosses
{
    public class TheConductor : Boss
    {
        public override WeaponType HittableBy { get; set; }
            = WeaponType.Sousaphone;

        public override WeaponType WeaponType { get; set; }
            = WeaponType.Sousaphone;


        /// <summary>
        /// The time between swapping instruments.
        /// </summary>
        private int _timeBetweenInstrumentSwapsInSeconds = 10;

        private float _timeElapsedSinceLastInstrumentSwapInSeconds = 0;

        protected override void OnEnable()
        {
            base.OnEnable();

            Behaviour.CurrentState.StateData.ProjectileCount = 3;
            Behaviour.CurrentState.StateData.ProjectileType = typeof(FinalBossNote);
            Behaviour.CurrentState.StateData.AttackController = new ConductorAttackController();
        }

        protected override void Update()
        {
            base.Update();
            
            //temporary to test 
            if (Keyboard.current.numpadPlusKey.wasPressedThisFrame)
                Behaviour.CurrentState.StateData.ProjectileCount++;
            if (Keyboard.current.numpadMinusKey.wasPressedThisFrame && Behaviour.CurrentState.StateData.ProjectileCount > 1)
                Behaviour.CurrentState.StateData.ProjectileCount--;

            _timeElapsedSinceLastInstrumentSwapInSeconds += Time.deltaTime;
            if (_timeElapsedSinceLastInstrumentSwapInSeconds >= _timeBetweenInstrumentSwapsInSeconds)
            {
                _timeElapsedSinceLastInstrumentSwapInSeconds = 0;
                SwapToRandomInstrument();
            }
        }

        private void SwapToRandomInstrument()
        {
            WeaponType[] instruments =
            {
                WeaponType.Guitar,
                WeaponType.Drum,
                WeaponType.Flute,
                WeaponType.Sousaphone
            };

            int randomIndex = Mathf.FloorToInt(Random.Range(0, 3.99f));

            HittableBy = WeaponType = instruments[randomIndex];

            ApplyVisibilityChangesForWeapon(HittableBy);
        }

        public override void OnEvent(EnemyDeathEvent enemyDeathEvent)
        {
            if (enemyDeathEvent.KilledEnemy.GetInstanceID() != GetInstanceID()) return;
            
            EventManager.Invoke(new FinalBossDeathEvent());
            Destroy(gameObject);
        }
        
    }
}