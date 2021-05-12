namespace OrchestraArmy.Entity.Entities.Enemies.Regular
{
    public class FluteEnemyFactory : IEnemyFactory
    {
        public Enemy MakeEnemy()
        {
            return new FluteEnemy();
        }

    }
}
