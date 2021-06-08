using System;

namespace OrchestraArmy.SaveData
{
    [Serializable]
    public class SettingsData
    {
        public float sound = 1.0f;
        public float gMusic = 1.0f;
        public float mMusic = 1.0f;
        public float beats = 1.0f;

        public int mouse = 0;
        public int dificulty = 1;
    }
}