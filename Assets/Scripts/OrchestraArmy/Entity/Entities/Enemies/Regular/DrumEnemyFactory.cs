namespace OrchestraArmy.Entity.Entities.Enemies.Regular
{
    public class DrumEnemyFactory : IEnemyFactory
    {
        public Enemy MakeEnemy()
        {
            return new DrumEnemy();
        }

    }
}
