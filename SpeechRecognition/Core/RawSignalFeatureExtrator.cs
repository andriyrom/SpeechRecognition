using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.Core {
    public class RawSignalFeatureExtrator : IFeatureExtractor {
        private ISpeechDetector SpeechDetector;

        public RawSignalFeatureExtrator()
            : this(new SimpleSpeechDetector()) { }

        public RawSignalFeatureExtrator(ISpeechDetector speechDetector) {
            SpeechDetector = speechDetector;
        }

        public FeaturesContainer GetFeatures(SignalContainer signal) {
            var features = signal.GetFrames().Select(frame => new FrameFeatures(frame.Bounds, frame.Signal)).
                Where(feature => feature.FeatureDimension == signal.FrameSize).ToList();
            return new FeaturesContainer(features);
        }
    }
}
