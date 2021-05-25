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

        /// <summary>
        /// Objects for the map surroundings
        /// </summary>
        public RhythmController(int newBPM)
        {
            RhythmData.BPM = newBPM;
            EventManager.Bind<EnteredNewLevelEvent>(this);
            RhythmData.SetStopwatch();

        }

        public RhythmController()
        {
            RhythmData.BPM = 120;
            EventManager.Bind<EnteredNewLevelEvent>(this);
            RhythmData.SetStopwatch();
        }

        public int StaminaDamage()
        {
            return RhythmData.GetStaminaDamage();
        }
        public int RhythmScore()
        {
            return RhythmData.GetRhythmScore();
        }

        

        public void GraduallyChangeBPM(object obj)
        {
            // cast object to newBPM
            int newBPM;
            try {
                newBPM = (int) obj;
            }
            catch (InvalidCastException) { // do nothing
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
                Thread.Sleep(20); // sleep 1/50th of a second
            }
        }
        
        public void ChangeBPM(int changeInPercentage)
        {
            int newBPM = RhythmData.BPM / 100 * changeInPercentage + RhythmData.BPM;
            
            // Start new thread for BPM change
            Thread t = new Thread(GraduallyChangeBPM);
            t.IsBackground = true;
            t.Start(newBPM);
        }

        public void OnEvent(EnteredNewLevelEvent invokedEvent)
        {
            RhythmData.SetStopwatch();
        }
    }
}