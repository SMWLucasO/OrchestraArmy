using System;
using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Entities.Players.WeaponSelection.Weapon.Weapons;
using OrchestraArmy.Entity.Entities.Projectiles;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace OrchestraArmy.Entity.Entities.Players.Controllers
{
    public class PlayerAttackController: IAttackController
    {
        public Player Player { get; set; }
        
        public void HandleAttack()
        {
            if (!Keybindings.KeybindingManager.Instance.Keybindings["Shoot"].wasPressedThisFrame)
                return;

            var obj = (GameObject) Object.Instantiate(Resources.Load("Prefabs/NoteProjectile"), Player.transform.position, Player.transform.GetChild(0).transform.rotation);
            var attack = obj.GetComponent<Note>();
            
            attack.transform.forward = Player.DirectionController.AimDirection;
            attack.Source = Player.transform.position;
            attack.Attacker = Player;
            attack.MaxDistance = 400;
            attack.Instrument = Player.WeaponWheel.CurrentlySelected.WeaponWheelPlaceholderData.Weapon;
            attack.Tone = Player.ToneController.CurrentTone;
            
            EventManager.Invoke(new PlayerAttackEvent() {Tone = Player.ToneController.CurrentTone, Instrument = Player.WeaponWheel.CurrentlySelected.WeaponWheelPlaceholderData.WeaponType});
        }
    }
}