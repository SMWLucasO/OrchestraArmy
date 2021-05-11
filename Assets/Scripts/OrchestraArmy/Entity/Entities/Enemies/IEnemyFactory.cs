using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OrchestraArmy.Entity.Entities.Enemies
{
    public interface IEnemyFactory
    {
        public Enemy MakeEnemy(int x, int y);
    }
}
