using UnityEngine;
using OrchestraArmy.Music.Data;
using System.Threading;
using OrchestraArmy.Event;
using OrchestraArmy.Event.Events.Level;
using System;

namespace OrchestraArmy.Music.Controllers
{
    public class RhythmController : IListener<EnteredNewLevelEvent>
    {
       

        public RhythmController()
        {
            EventManager.Bind<EnteredNewLevelEvent>(this);
            RhythmData.SetStopwatch();
        }

        /// <summary>
        /// Get stamina damage in negative int
        /// </summary>
        public int GetStaminaDamage()
        {
            return RhythmData.GetStaminaDamage();
        }

        /// <summary>
        /// Get the rhythm score between 0 and 100
        /// </summary>
        public int GetRhythmScore()
        {
            return RhythmData.GetRhythmScore();
        }

        // DOESN'T WORK, FIX LATER

        /// <summary>
        /// Gradually change bpm for a better flow
        /// </summary>
        private void GraduallyChangeBPM(object obj)
        {
            // cast object to newBPM
            int newBPM;
            try {
                newBPM = (int) obj;
            }
            catch (InvalidCastException) { 
                // do nothing
                newBPM = RhythmData.BPM;
            }

            // start changing bpm
            while(true)
            {
                if(RhythmData.BPM>newBPM)
                {
                    RhythmData.BPM--;
                }
                else if(RhythmData.BPM<newBPM)
                {
                    RhythmData.BPM++;
                }
                else
                {
                    break;
                }
                // sleep 1/50th of a second
                Thread.Sleep(200); 
            }
        }
        
        /// <summary>
        /// Public starting point for GraduallyChangeBPM
        /// </summary>
        public void ChangeBPMByPercentage(int changeInPercentage)
        {
            int newBPM = (int)(RhythmData.BPM / 100f * changeInPercentage + RhythmData.BPM);
            
            // Start new thread for BPM change
            Thread t = new Thread(GraduallyChangeBPM);
            t.IsBackground = true;
            t.Start(newBPM);
        }

        /// <summary>
        /// Public starting point for GraduallyChangeBPM
        /// </summary>
        public void ChangeBPMByBPM(int changeInBPM)
        {
            // Start new thread for BPM change
            Thread t = new Thread(GraduallyChangeBPM);
            t.IsBackground = true;
            t.Start(changeInBPM);
        }

        /// <summary>
        /// Change BPM Immediately
        /// </summary>
        public void ChangeBPMImmediately(int changeInBPM)
        {
            RhythmData.BPM = changeInBPM;
        }

        public void OnEvent(EnteredNewLevelEvent invokedEvent)
        {
            // Behaviour not specified yet
        }
    }
}