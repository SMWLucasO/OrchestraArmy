using OrchestraArmy.Entity.Entities.Players;

namespace OrchestraArmy.Entity.Controllers
{
    public interface IWeaponSelectionController
    {
        /// <summary>
        /// The player to handle the weapon selection for.
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// Handler for the weapon selection.
        /// </summary>
        public void HandleWeaponSelection();
    }
}