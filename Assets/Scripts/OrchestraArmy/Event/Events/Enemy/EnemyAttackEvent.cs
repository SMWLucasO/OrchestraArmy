using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons.Factory;
using OrchestraArmy.Enum;
using UnityEngine;

namespace OrchestraArmy.Event.Events.Enemy
{
    public class EnemyAttackEvent : IEvent
    {
        public WeaponType Instrument;
        public Tone Tone;
        public Vector3 Position;
    }
}