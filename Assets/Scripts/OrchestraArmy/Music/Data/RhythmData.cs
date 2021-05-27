using UnityEngine;
using System.Diagnostics;
using System;

namespace OrchestraArmy.Music.Data
{
    public static class RhythmData
    {
        
        /// <summary>
        /// Rhythm stopwatch
        /// </summary>
        private static Stopwatch _rhythmStopwatch;


        /// <summary>
        /// Returns the damage to the stamina
        /// </summary>
        public static int GetStaminaDamage(int BPM)
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
        public static int GetRhythmScore(int BPM)
        {
            // Get elapsed time in seconds
            TimeSpan timeSpan = _rhythmStopwatch.Elapsed;
            double sTime = timeSpan.TotalSeconds;

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