using OrchestraArmy.Entity.Entities.Enemies;

namespace OrchestraArmy.Entity.Controllers
{
    public interface IEnemyAttackController: IAttackController
    {
        public Enemy Enemy { get; set; }
    }
}