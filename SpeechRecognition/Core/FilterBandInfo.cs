using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.Core {
    public class FilterBandInfo {
        public int LowFrequency { get; private set; }

        public int HighFrequency { get; private set; }

        public FilterBandInfo(int lowFrequency, int highFrequency) {
            LowFrequency = lowFrequency;
            HighFrequency = highFrequency;
        }
    }
}
