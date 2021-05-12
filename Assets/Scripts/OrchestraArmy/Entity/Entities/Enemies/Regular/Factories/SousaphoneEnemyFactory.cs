namespace OrchestraArmy.Entity.Entities.Enemies.Regular.Factories
{
    public class SousaphoneEnemyFactory : IEnemyFactory
    {
        public Enemy MakeEnemy()
        {
            return new SousaphoneEnemy();
        }

    }
}
