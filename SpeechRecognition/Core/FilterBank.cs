using MathNet.Filtering.FIR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.Core {
    class FilterBank {
        private List<FilterInfo> Filters;
        private double SamplingRate;

        public FilterBank(int startFrequency, int bandWidth, int filtersCount, double samplingRate) {
            SamplingRate = samplingRate;
            CreateFilters(startFrequency, bandWidth, filtersCount);
        }

        private void CreateFilters(int startFrequency, int bandWidth, int filtersCount) {
            Filters = new List<FilterInfo>();
            int filterLowFrequency = startFrequency;
            for (int i = 0; i < filtersCount; i++) {
                int filterHightFrequency = filterLowFrequency + bandWidth;
                FilterInfo filter = CreateFilter(filterLowFrequency, filterHightFrequency);
                Filters.Add(filter);
                filterLowFrequency = filterHightFrequency;
            }
        }

        private FilterInfo CreateFilter(int lowFrequency, int highFrequency) {
            double[] coefficients = FirCoefficients.BandPass(SamplingRate, lowFrequency, highFrequency, 100);
            OnlineFirFilter filter = new OnlineFirFilter(coefficients);
            return new FilterInfo(filter, lowFrequency, highFrequency);  
        }

        public List<FilterProcessResult> ProcessSignal(SignalContainer input) {
            List<FilterProcessResult> processResult = new List<FilterProcessResult>();
            foreach (FilterInfo filterInfo in Filters) {
                double[] processedSignal = filterInfo.Filter.ProcessSamples(input.Signal);
                processResult.Add(new FilterProcessResult(filterInfo.Band, processedSignal));
            }
            return processResult;
        }

        private class FilterInfo {
            public OnlineFirFilter Filter { get; private set; }

            public FilterBandInfo Band { get; private set; }

            public FilterInfo(OnlineFirFilter filter, int lowFrequency, int highFrequency) {
                Filter = filter;
                Band = new FilterBandInfo(lowFrequency, highFrequency);               
            }
        }
    }
}
