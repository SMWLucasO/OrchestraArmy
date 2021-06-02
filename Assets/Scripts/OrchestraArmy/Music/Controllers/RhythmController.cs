using UnityEngine;
using OrchestraArmy.Music.Data;
using System.Threading;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Level;
using System;
using System.Diagnostics;


namespace OrchestraArmy.Music.Controllers
{
    public class RhythmController : IListener<EnteredNewLevelEvent>
    {

        /// <summary>
        /// Rhythm stopwatch
        /// </summary>
        private static Stopwatch _rhythmStopwatch;

        public RhythmController()
        {
            EventManager.Bind<EnteredNewLevelEvent>(this);
            SetStopwatch();
        }

        /// <summary>
        /// Returns the damage to the stamina in negative float
        /// </summary>
        public double GetStaminaDamage(int BPM)
        {
            // Get elapsed time in seconds
            TimeSpan timeSpan = _rhythmStopwatch.Elapsed;
            double sTime = timeSpan.TotalSeconds;

            // Calculate damage and return
            return (Math.Cos(sTime*Math.PI*(BPM/60f)+Math.PI)-1)/4;
            
        }

        //// <summary>
        /// Returns value from 1 to 100 indicating what the score is right now, 100 is good, 1 is bad.
        /// </summary>
        public int GetRhythmScore(int BPM)
        {
            // Get elapsed time in seconds
            TimeSpan timeSpan = _rhythmStopwatch.Elapsed;
            double sTime = timeSpan.TotalSeconds;

            return (int)(100 * ((Math.Cos(sTime*Math.PI*(BPM/60f)+Math.PI)+1)/2));

        }

        /// <summary>
        /// Set the rhythm stopwatch
        /// </summary>
        public void SetStopwatch()
        {
            // only set if not existing, or you reset the current stopwatch
            if(_rhythmStopwatch == null) 
            {
                _rhythmStopwatch = new Stopwatch();
                _rhythmStopwatch.Start();
            }
            
        }


        public void OnEvent(EnteredNewLevelEvent invokedEvent)
        {
            // Behaviour not specified yet
        }
    }
}