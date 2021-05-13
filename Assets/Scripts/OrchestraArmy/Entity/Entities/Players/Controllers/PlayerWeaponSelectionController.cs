using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Keybindings;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Players.Controllers
{
    public class PlayerWeaponSelectionController : IWeaponSelectionController
    {
        
        public Player Player { get; set; }
        
        public void HandleWeaponSelection()
        {
            // switch to the next instrument
            if (Input.GetKeyDown(KeybindingManager.Instance.Keybindings["Select next instrument"]))
                Player.WeaponWheel.SwitchToNextWeapon();
            
            // switch to the previous weapon
            else if (Input.GetKeyDown(KeybindingManager.Instance.Keybindings["Select previous instrument"]))
                Player.WeaponWheel.SwitchToPreviousWeapon();
        }
    }
}