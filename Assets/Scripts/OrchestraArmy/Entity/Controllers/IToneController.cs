using OrchestraArmy.Enum;

namespace OrchestraArmy.Entity.Controllers
{
    public interface IToneController
    {
        /// <summary>
        /// The currently selected tone
        /// </summary>
        public Tone CurrentTone { get; }
        
        /// <summary>
        /// Handle tone updates
        /// </summary>
        public void HandleTone();
    }
}