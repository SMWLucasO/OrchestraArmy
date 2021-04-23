using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Entity.Entities.Player.Controllers;
using UnityEngine;

namespace OrchestraArmy.Entity.Entities.Player
{
    public class Player : LivingDirectionalEntity
    {
        
        public ICameraController CameraController;

        public Player()
        {
            this.MovementController = new PlayerMovementController()
            {
                Entity = this
            };
        }
        
    }
}