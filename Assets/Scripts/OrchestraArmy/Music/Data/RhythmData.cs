using UnityEngine;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;

namespace OrchestraArmy.Music.Data
{
    public static class RhythmData
    {
        
        [Min(0)]
        [SerializeField]
        private static int _BPM = 120;

        /// <summary>
        /// Rhythm stopwatch
        /// </summary>
        private static Stopwatch _rhythmStopwatch;

        public static float ElapsedSeconds => (float)_rhythmStopwatch.Elapsed.TotalSeconds;
        public static int CurrentBeat = 0;
        public static float LastChangedBeat = Time.time;

        /// <summary>
        /// BPM
        /// </summary>
        public static int BPM
        {
            get => _BPM;
            set => _BPM = value;
        }

        /// <summary>
        /// Returns the damage to the stamina
        /// </summary>
        public static int GetStaminaDamage()
        {
            int maxStamina = 100;
            // Get elapsed time in seconds
            TimeSpan timeSpan = _rhythmStopwatch.Elapsed;
            double sTime = timeSpan.TotalSeconds;

            // Calculate damage and return
            return (int)(maxStamina * ((Math.Cos(sTime*Math.PI*(BPM/60f)+Math.PI)-1)/4));
            
        }

        
        /// <summary>
        /// Returns value from 1 to 100 indicating what the score is right now
        /// </summary>
        public static int GetRhythmScore()
        {
            // Get elapsed time in seconds
            TimeSpan timeSpan = _rhythmStopwatch.Elapsed;
            double sTime = Time.time;

            return (int)(100 * ((Math.Cos(sTime*Math.PI*(BPM/60f)+Math.PI)+1)/2));
        }

        /// <summary>
        /// Set the rhythm stopwatch
        /// </summary>
        public static void SetStopwatch()
        {
            // only set if not existing, or you reset the current stopwatch
            if(_rhythmStopwatch == null) 
            {
                _rhythmStopwatch = new Stopwatch();
                _rhythmStopwatch.Start();
            }
            
        }


    }
}