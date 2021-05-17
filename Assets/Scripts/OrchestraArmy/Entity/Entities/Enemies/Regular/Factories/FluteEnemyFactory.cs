namespace OrchestraArmy.Entity.Entities.Enemies.Regular.Factories
{
    public class FluteEnemyFactory : IEnemyFactory
    {
        public Enemy MakeEnemy()
        {
            return new FluteEnemy();
        }

    }
}
