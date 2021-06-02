using UnityEngine;
using OrchestraArmy.Music.Data;
using System.Threading;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Level;
using System;
using System.Diagnostics;
using System.Collections;
using OrchestraArmy.Event.Events.Player;
using OrchestraArmy.Event.Events.Rhythm;

namespace OrchestraArmy.Music.Controllers
{
    public class RhythmController : IListener<EnteredNewLevelEvent>
    {
        private bool _started = false;

        /// <summary>
        /// Rhythm stopwatch
        /// </summary>
        private static Stopwatch _rhythmStopwatch;

        public RhythmController()
        {
            EventManager.Bind<EnteredNewLevelEvent>(this);
            SetStopwatch();
        }

        public IEnumerator BeatCheck()
        {
            while (true)
            {
                var score = RhythmData.GetRhythmScore();
                            
                if (score >= 99 && RhythmData.CurrentBeat % 2 == 1 || score <= 1 && RhythmData.CurrentBeat % 2 == 0)
                {
                    RhythmData.CurrentBeat = RhythmData.CurrentBeat % 4 + 1;
                    
                    if (RhythmData.CurrentBeat % 2 == 1)
                        EventManager.Invoke(new OffBeatEvent());
                    else
                        EventManager.Invoke(new EvenBeatEvent());
                }

                yield return new WaitForSeconds(0.01f);
            }
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