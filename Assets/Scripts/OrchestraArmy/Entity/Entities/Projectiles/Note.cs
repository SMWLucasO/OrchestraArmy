using System;
using OrchestraArmy.Entity.Entities.Players;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Player;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Projectiles
{
    public class Note : Projectile
    {
        public IWeapon Instrument { get; set; }
        public Player Attacker { get; set; }

        public void OnCollisionEnter(Collision collision)
        {
            switch (collision.collider.tag)
            {
                case "Player":
                    Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
                    break;
                case "World":
                    Hit = true;
                    break;
                case "Enemy":
                    Hit = true;
                    EventManager.Invoke(new PlayerAttackHitEvent()
                    {
                        Attacker = Attacker,
                        TargetId = collision.collider.gameObject.GetInstanceID(),
                        Weapon = Instrument
                    });
                    break;
            }
        }
    }
}