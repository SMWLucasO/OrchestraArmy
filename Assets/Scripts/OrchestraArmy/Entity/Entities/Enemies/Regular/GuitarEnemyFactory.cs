using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OrchestraArmy.Entity.Entities.Enemies.Regular
{
    public class GuitarEnemyFactory : IEnemyFactory
    {
        public Enemy MakeEnemy(int x, int y)
        {
            return new GuitarEnemy();
        }

    }
}
