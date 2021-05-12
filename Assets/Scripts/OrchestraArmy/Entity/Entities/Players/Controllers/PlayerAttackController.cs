using System;
using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons;
using OrchestraArmy.Entity.Entities.Projectiles;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OrchestraArmy.Entity.Entities.Players.Controllers
{
    public class PlayerAttackController: IAttackController
    {
        public Player Player { get; set; }
        
        public void HandleAttack()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            var obj = (GameObject) Object.Instantiate(Resources.Load("Prefabs/NoteProjectile"), Player.transform.position, Player.transform.GetChild(0).transform.rotation);
            var attack = obj.GetComponent<Note>();
            
            attack.transform.forward = Player.transform.forward;
            attack.Source = Player.transform.position;
            attack.Speed = 5;
            attack.Attacker = Player;
            attack.MaxDistance = 400;
            attack.Instrument = Player.WeaponWheel.CurrentlySelected.WeaponWheelPlaceholderData.Weapon;
        }
    }
}