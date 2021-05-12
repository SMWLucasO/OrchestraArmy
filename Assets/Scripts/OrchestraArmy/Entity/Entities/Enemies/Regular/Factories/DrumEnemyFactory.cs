namespace OrchestraArmy.Entity.Entities.Enemies.Regular.Factories
{
    public class DrumEnemyFactory : IEnemyFactory
    {
        public Enemy MakeEnemy()
        {
            return new DrumEnemy();
        }

    }
}
