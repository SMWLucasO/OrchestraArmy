using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrchestraArmy.Enum;

namespace OrchestraArmy.Music.Instruments
{

    [System.Serializable]
    public struct Range
    {
        [Range(0,3)]
        public int LowestOctave;
        [Range(3,5)]
        public int HighestOctave;
    }

    public class InstrumentData : MonoBehaviour
    {

        public Range Range;

        public Tone BaseTone;
        
    }
}
