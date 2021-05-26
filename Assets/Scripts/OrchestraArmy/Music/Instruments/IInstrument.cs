using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrchestraArmy.Music.Instruments
{

    public struct Range
    {
        public int LowestOctave;
        public int HighestOctave;
    }
    
    public class IInstrument
    {

        public Range Range;
        
    }
}
