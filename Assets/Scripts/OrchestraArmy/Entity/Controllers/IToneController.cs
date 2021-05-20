using OrchestraArmy.Enum;

namespace OrchestraArmy.Entity.Controllers
{
    public interface IToneController
    {
        public Tone CurrentTone { get; }
        public void HandleTone();
    }
}