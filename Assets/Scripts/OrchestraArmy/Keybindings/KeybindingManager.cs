using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace OrchestraArmy.Keybindings
{
    public class KeybindingManager
    {

        public static KeybindingManager Instance { get; set; }
            = new KeybindingManager();
        
        /// <summary>
        /// Store for all the keybindings, where the description points to the bound key.
        /// </summary>
        public Dictionary<string, ButtonControl> Keybindings => GetDefaultKeybindings();

        /// <summary>
        /// Get the default keybindings.
        /// </summary>
        private Dictionary<string, ButtonControl> GetDefaultKeybindings()
            => new Dictionary<string, ButtonControl>()
            {
                {"Move forward", Keyboard.current.wKey},
                {"Move backward", Keyboard.current.sKey},
                {"Move left", Keyboard.current.aKey},
                {"Move right", Keyboard.current.dKey},
                {"Rotate camera left", Keyboard.current.leftArrowKey},
                {"Rotate camera right", Keyboard.current.rightArrowKey},
                {"Select next instrument", Keyboard.current.eKey},
                {"Select previous instrument", Keyboard.current.qKey},
                {"Shoot", Mouse.current.leftButton},
            };
    }
}