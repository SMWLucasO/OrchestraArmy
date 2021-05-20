namespace OrchestraArmy.Room.Rooms
{
    public class BossRoom : Room
    {
        public override void SetupSettings()
        {
            RoomGenerationData.WalkerDirectionChangeChance = .99f;
            RoomGenerationData.WalkerSpawnChance = 0.8f;
            RoomGenerationData.WalkerDestructionChance = 0.8f;
            RoomGenerationData.MaxAliveWalkers = 20;
            RoomGenerationData.PercentToFill = 0.1f;
        }
    }
}