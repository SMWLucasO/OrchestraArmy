using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrchestraArmy.Enum;

namespace OrchestraArmy.Music.Instruments
{
    public class InstrumentData : MonoBehaviour
    {
        public Tone BaseTone = Tone.C;

        public Interval Interval = Interval.Quarters;


        //The chance a note will play on its interval
        [Range(0,100)]
        public int Chance = 100;

        public float SpecificVolume = 1f;
    }
}
