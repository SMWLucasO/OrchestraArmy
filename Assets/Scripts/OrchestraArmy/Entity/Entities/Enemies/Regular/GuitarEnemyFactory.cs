namespace OrchestraArmy.Entity.Entities.Enemies.Regular
{
    public class GuitarEnemyFactory : IEnemyFactory
    {
        public Enemy MakeEnemy()
        {
            return new GuitarEnemy();
        }

    }
}
