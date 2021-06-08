using OrchestraArmy.Entity.Entities.Enemies;

namespace OrchestraArmy.Entity.Controllers
{
    /// <summary>
    /// Attack controllers for enemies.
    /// </summary>
    public interface IEnemyAttackController: IAttackController
    {
        /// <summary>
        /// The attacking enemy
        /// </summary>
        public Enemy Enemy { get; set; }
    }
}