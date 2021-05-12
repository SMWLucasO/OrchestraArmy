namespace OrchestraArmy.Entity.Entities.Enemies.Regular
{
    public class SousaphoneEnemyFactory : IEnemyFactory
    {
        public Enemy MakeEnemy()
        {
            return new SousaphoneEnemy();
        }

    }
}
