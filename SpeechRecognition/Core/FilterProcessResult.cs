using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.Core {
    class FilterProcessResult {
        public FilterBandInfo Band { get; private set; }

        public double[] FilteredSignal { get; private set; }

        public FilterProcessResult(FilterBandInfo band, double[] filteredSignal) {
            Band = band;
            FilteredSignal = filteredSignal;
        }
    }
}
