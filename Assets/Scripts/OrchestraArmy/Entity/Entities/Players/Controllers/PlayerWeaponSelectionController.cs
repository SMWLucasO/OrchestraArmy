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
            // Not allowed to press both buttons at the same time.
            if (Input.GetKeyDown(KeybindingManager.Instance.Keybindings["Select next instrument"])
                && Input.GetKeyDown(KeybindingManager.Instance.Keybindings["Select previous instrument"]))
                return;
            
            // Switch to the next instrument
            if (Input.GetKeyDown(KeybindingManager.Instance.Keybindings["Select next instrument"]))
                Player.WeaponWheel.SwitchToNextWeapon();
            
            // Switch to the previous weapon
            else if (Input.GetKeyDown(KeybindingManager.Instance.Keybindings["Select previous instrument"]))
                Player.WeaponWheel.SwitchToPreviousWeapon();
        }
    }
}