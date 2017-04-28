using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.Core {
    public class RepeatFilterBankFeatureExtractor : IFeatureExtractor {
        private FilterBank FilterBankCore;
        private int RepeatFrameTimes;
        private int TakeFrameNumber;

        public RepeatFilterBankFeatureExtractor(FilterBankParameters filterBankSettings, int repeatFrameTimes, int takeFrameNumber) {
            FilterBankCore = new FilterBank(filterBankSettings.StartFrequency,
                filterBankSettings.BandWidth,
                filterBankSettings.Count,
                filterBankSettings.SampleRate,
                filterBankSettings.Order);
            RepeatFrameTimes = repeatFrameTimes;
            TakeFrameNumber = takeFrameNumber;
        }

        public FeaturesContainer GetFeatures(SignalContainer signal) {
            IList<Frame> frames = signal.GetFrames();
            List<FrameFeatures> features = new List<FrameFeatures>();
            foreach (Frame frame in frames) {
                SignalContainer extendedSignal = GetSignalForFiltering(frame.Signal);
                IList<FilterProcessResult> extendedSignalChannels = FilterBankCore.ProcessSignal(extendedSignal);
                FilterBankCore.Reset();
                IList<FilterProcessResult> signalChannels = GetFilteredSignalsPart(extendedSignalChannels, frame.Bounds.Width);
                double[] channelsNormalizedEnergy = GetChannelsEnergy(signalChannels);
                features.Add(new FrameFeatures(frame.Bounds, channelsNormalizedEnergy));
            }
            return new FeaturesContainer(features);
        }

        private SignalContainer GetSignalForFiltering(double[] rawSignal) {
            double[] extendedSignal = Enumerable.Repeat(rawSignal, RepeatFrameTimes)
                .SelectMany(array => array).ToArray();
            return new SignalContainer(extendedSignal);
        }

        private List<FilterProcessResult> GetFilteredSignalsPart(IList<FilterProcessResult> extendedResults, int originalSignalLength) {
            return extendedResults.Select(channelSignal => GetFilteredSingalForOutput(channelSignal, originalSignalLength)).ToList();
        }

        private FilterProcessResult GetFilteredSingalForOutput(FilterProcessResult extendedResult, int originalSignalLength) {
            double[] filteredSignal = extendedResult.FilteredSignal.Skip(TakeFrameNumber * originalSignalLength)
                .Take(originalSignalLength).ToArray();
            return new FilterProcessResult(extendedResult.Band, filteredSignal);
        }

        private double[] GetChannelsEnergy(IList<FilterProcessResult> channelsSignal) {
            var channelsRawEnergy = channelsSignal.Select(channel => SignalHelper.GetEnergy(channel.FilteredSignal, 0, channel.FilteredSignal.Length - 1));
            double maxEnergy = channelsRawEnergy.Max(channel => Math.Abs(channel));
            return channelsRawEnergy.Select(channelEnergy => channelEnergy / maxEnergy).ToArray();
        }
    }
}
