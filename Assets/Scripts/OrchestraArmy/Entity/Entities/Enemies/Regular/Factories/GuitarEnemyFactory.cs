namespace OrchestraArmy.Entity.Entities.Enemies.Regular.Factories
{
    public class GuitarEnemyFactory : IEnemyFactory
    {
        public Enemy MakeEnemy()
        {
            return new GuitarEnemy();
        }

    }
}
