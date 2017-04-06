using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.Core {
    public class SpeechBounds {
        public int Start { get; private set; }

        public int End { get; private set; }

        public int Width {
            get {
                return End - Start + 1;
            }
        }

        public SpeechBounds(int start, int end) {
            Start = start;
            End = end;
        }
    }
}
