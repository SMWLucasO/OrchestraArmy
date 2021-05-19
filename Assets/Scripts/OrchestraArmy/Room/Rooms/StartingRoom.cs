using UnityEngine;

namespace OrchestraArmy.Room.Rooms
{
    public class StartingRoom : Room
    {
        public override void Generate()
        {
            CreateSpawn(); 
            CreateWalls();
            CreateDoors();
        }
        
        public override void SetupSettings() { }
        
        void CreateSpawn()  //creates the spawn room layout 
        {
            this.RoomIsCleared = false;
            //find grid size
            RoomHeight = Mathf.RoundToInt(RoomSizeWorldUnits.x / RoomGenerationData.WorldUnitsInOneGridCell);
            RoomWidth = Mathf.RoundToInt(RoomSizeWorldUnits.y / RoomGenerationData.WorldUnitsInOneGridCell);
            
            //create grid
            Grid = new GridSpace[RoomWidth, RoomHeight];
            Vector2 center = RoomSizeWorldUnits / 2;
            for (int x = (int)-center.x; x < center.x; x++) {
                for (int y = (int)-center.y; y < center.y; y++) {
                    if (x*x+y*y<=100) {
                        Grid[(int) (x+center.x),(int) (y+center.y)] = GridSpace.Floor;
                    } else {
                        Grid[(int)(x+center.x),(int)(y+center.y)] = GridSpace.Empty;
                    }
                }
            }
        }
        
    }
}