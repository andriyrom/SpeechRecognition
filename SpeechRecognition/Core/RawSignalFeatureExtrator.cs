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
            int initialFrameSize = signal.FrameSize;
            SignalContainer speech = SpeechDetector.GetSpeech(signal);
            speech.FrameSize = initialFrameSize;
            var features = speech.GetFrames().Select(frame => new FrameFeatures(frame.Bounds, frame.Signal)).
                Where(feature => feature.FeatureDimension == initialFrameSize).ToList();
            return new FeaturesContainer(features);
        }
    }
}
