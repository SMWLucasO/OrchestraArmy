using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrchestraArmy.Enum;

namespace OrchestraArmy.Music.Instruments
{
    public class InstrumentData : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public Tone BaseTone = Tone.C;

        /// <summary>
        /// 
        /// </summary>
        public Interval Interval = Interval.Quarters;
        
        /// <summary>
        /// The chance a note will play on its interval
        /// </summary>
        [Range(0,100)]
        public int Chance = 100;

        /// <summary>
        /// To assign a specific value to an instrument in case it's audio file is very loud or very silent.
        /// </summary>
        public float SpecificVolume = .5f;
    }
}
