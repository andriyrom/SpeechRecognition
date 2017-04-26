using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.Core {
    public class FilterBankParameters {
        public int StartFrequency { get;  private set; }
 
        public int BandWidth { get;  private set; }
 
        public int Count { get;  private set; }
 
        public double SampleRate { get;  private set; }

        public int Order { get; private set; }

        public FilterBankParameters(int startFrequency, int bandWidth, int filterCount, double sampleRate, int filterOrder) {
            StartFrequency = startFrequency;
            BandWidth = bandWidth;
            Count = filterCount;
            SampleRate = sampleRate;
            Order = filterOrder;
        }
    }
}
