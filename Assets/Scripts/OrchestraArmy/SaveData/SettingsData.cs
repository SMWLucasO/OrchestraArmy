using System;

namespace OrchestraArmy.SaveData
{
    [Serializable]
    public class SettingsData
    {
        public float Sound = 1.0f;
        public float GMusic = 1.0f;
        public float MMusic = 1.0f;
        public float Beats = 1.0f;

        public int Mouse = 0;
        public int Difficulty = 1;
    }
}