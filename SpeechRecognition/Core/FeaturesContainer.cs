using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.Core {
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

        public List<double[]> GetSeparately() {
            return SignalFeatures.Select(frameFeatures => frameFeatures.Features).ToList();
        }
    }
}
