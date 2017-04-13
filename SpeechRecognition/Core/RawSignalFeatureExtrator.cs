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
            SignalContainer speech = SpeechDetector.GetSpeech(signal);
            var speechBounds = new SpeechBounds(0, speech.Signal.Length - 1);
            var features = new FrameFeatures(speechBounds, speech.Signal);
            return new FeaturesContainer(new List<FrameFeatures>() { features });
        }
    }
}
