using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.Core {
    public enum SplitContainerMode {
        DiscardFractionalPart = 0,
        ExtendFractionalPart = 1
    }

    public class FeaturesContainer {
        private List<FrameFeatures> SignalFeatures;

        public int FramesCount {
            get {
                return SignalFeatures.Count;
            }
        }

        public FeaturesContainer(IEnumerable<FrameFeatures> features) {
            if (!IsFeaturesSizeIdentical(features)) {
                throw new ArgumentException("Feature vectors should have the same size.");
            }
            SignalFeatures = new List<FrameFeatures>(features);
        }

        private bool IsFeaturesSizeIdentical(IEnumerable<FrameFeatures> features) {
            int standartFeatureDimension = features.First().FeatureDimension;
            return features.All(frame => frame.FeatureDimension == standartFeatureDimension);
        }

        public double[] GetSequential() {
            List<double[]> rawFeatures = GetSeparately();
            int featureSize = SignalFeatures.First().FeatureDimension;
            double[] sequentialFeatures = new double[featureSize * rawFeatures.Count];
            for (int i = 0; i < rawFeatures.Count; i++) {
                int destinationIndex = i * featureSize;
                Array.Copy(rawFeatures[i], 0, sequentialFeatures, destinationIndex, featureSize);
            }
            return sequentialFeatures;
        }

        public SpeechBounds GetBounds() {
            int start = SignalFeatures.First().FrameBounds.Start;
            int end = SignalFeatures.Last().FrameBounds.End;
            return new SpeechBounds(start, end);
        }

        public List<double[]> GetSeparately() {
            return SignalFeatures.Select(frameFeatures => frameFeatures.Features).ToList();
        }

        public List<FeaturesContainer> SplitIntoContainers(int frameSize, int frameShift, SplitContainerMode lastPartAction) {
            List<FeaturesContainer> splittedContainers = new List<FeaturesContainer>();
            int startIndex = 0;
            int endIndex = frameSize - 1;
            int lastFeatureFrameIndex = SignalFeatures.Count - 1;
            while (endIndex < lastFeatureFrameIndex) {
                splittedContainers.Add(GetCurrentContainer(startIndex, endIndex, frameSize));
                startIndex += frameShift;
                endIndex = startIndex + frameSize - 1;                
            }
            if (endIndex == lastFeatureFrameIndex) {
                return splittedContainers;
            }
            if (lastPartAction == SplitContainerMode.ExtendFractionalPart) {
                splittedContainers.Add(GetExtendedContainer(startIndex, endIndex, lastFeatureFrameIndex));
            }
            return splittedContainers;
        }

        private FeaturesContainer GetCurrentContainer(int startIndex, int endIndex, int frameSize) {
            IEnumerable<FrameFeatures> currentContainerFeatures = SignalFeatures.Skip(startIndex).Take(frameSize);
            return new FeaturesContainer(currentContainerFeatures);
        }

        private FeaturesContainer GetExtendedContainer(int startIndex, int endIndex, int lastFeatureFrameIndex) {
            var partFeatures = SignalFeatures.Skip(startIndex);
            FrameFeatures lastFrame = partFeatures.Last();
            var extendedPart = Enumerable.Repeat(lastFrame, endIndex - lastFeatureFrameIndex);
            List<FrameFeatures> extendedContainerFeatures = partFeatures.Concat(extendedPart).ToList();
            return new FeaturesContainer(extendedContainerFeatures);
        }
    }
}
