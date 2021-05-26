using OrchestraArmy.Entity.Entities.Enemies;
using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon;
using OrchestraArmy.Enum;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Enemy;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Projectiles
{
    public class EnemyNote : Projectile
    {
        public IWeapon Instrument { get; set; }
        public Enemy Attacker { get; set; }
        public Tone Tone { get; set; }

        public void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Player"))
            {
                Hit = true;
                EventManager.Invoke(new EnemyAttackHitEvent()
                {
                    Attacker = Attacker,
                    Target = collider.gameObject.GetComponent<Player>(),
                    Weapon = Instrument,
                    Tone = Tone
                });
            }
            else
            {
                Hit = true;
            }
        }
    }
}