using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.Core {
    public interface IFeatureExtractor {
        FeaturesContainer GetFeatures(SignalContainer signal);
    }
}
