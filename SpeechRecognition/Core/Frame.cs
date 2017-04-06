using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.Core {
    public class Frame {

        public Frame(double[] signal, SpeechBounds bounds) {
            Signal = new double[signal.Length];
            signal.CopyTo(Signal, 0);
            Bounds = bounds;
        }

        public double[] Signal { get; private set; }
        public SpeechBounds Bounds { get; private set; }
    }
}
