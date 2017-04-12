using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.Core {
    public class FrameFeatures {
        public SpeechBounds FrameBounds { get; private set; }

        public int FeatureDimension { 
            get {
                return vFeatures.Length;
            } 
        }

        public double[] Features { 
            get {
                double[] features = new double[vFeatures.Length];
                vFeatures.CopyTo(features, 0);
                return features;
            } 
        }
        private double[] vFeatures;

        public FrameFeatures(SpeechBounds frameBounds, double[] features) {
            FrameBounds = frameBounds;
            vFeatures = new double[features.Length];
            features.CopyTo(vFeatures, 0);
        }
    }
}
