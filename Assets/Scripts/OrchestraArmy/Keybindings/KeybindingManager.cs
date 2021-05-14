using System.Collections.Generic;
using UnityEngine;

namespace OrchestraArmy.Keybindings
{
    public class KeybindingManager
    {

        public static KeybindingManager Instance { get; set; }
            = new KeybindingManager();
        
        /// <summary>
        /// Store for all the keybindings, where the description points to the bound key.
        /// </summary>
        public Dictionary<string, KeyCode> Keybindings { get; set; }

        private KeybindingManager()
        {
            Keybindings = GetDefaultKeybindings();
        }

        /// <summary>
        /// Get the default keybindings.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, KeyCode> GetDefaultKeybindings()
            => new Dictionary<string, KeyCode>()
            {
                {"Move forward", KeyCode.W},
                {"Move backward", KeyCode.S},
                {"Move left", KeyCode.A},
                {"Move right", KeyCode.D},
                {"Rotate camera left", KeyCode.LeftArrow},
                {"Rotate camera right", KeyCode.RightArrow},
                {"Select next instrument", KeyCode.E},
                {"Select previous instrument", KeyCode.Q},
            };
    }
}