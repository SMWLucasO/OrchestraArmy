using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Players.WeaponSelection
{
    public class WeaponWheel : MonoBehaviour
    {
        
        /// <summary>
        /// The weapon wheel's currently selected placeholder.
        /// </summary>
        [field: SerializeField]
        public WeaponWheelPlaceholder CurrentlySelected { get; set; }
        
    }
}
