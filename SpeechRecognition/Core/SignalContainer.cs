using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.Core {
    public class SignalContainer {
        public double[] Signal { 
            get {
                if (vSignal == null) {
                    vSignal = LinkInSignal(Frames);
                }
                return vSignal;
            }            
        }
        private double[] vSignal;
             
        public int FrameSize { get; set; }

        private SignalContainer(SignalContainer other) : this(other.Signal) {
            FrameSize = other.FrameSize;
            Frames = other.Frames == null ? null : new List<Frame>(other.Frames);
        }

        public SignalContainer(double[] signal) {
            vSignal = new double[signal.Length];
            signal.CopyTo(Signal, 0);
            FrameSize = Signal.Length;
        }

        public SignalContainer(IList<Frame> frames) {
            int frameWidth = frames[0].Bounds.Width;
            bool framesWithSameWidth = frames.All(frame => frame.Bounds.Width == frameWidth);
            if (!framesWithSameWidth) {
                throw new ArgumentException("Each frame should have the same width.");
            }
            Frames = new List<Frame>(frames);
        }

        public List<Frame> GetFrames() {
            if (Frames == null || Frames[0].Bounds.Width != FrameSize) {
                Frames = SplitIntoFrames(Signal, FrameSize);
            }
            return new List<Frame>(Frames);
        }
        private List<Frame> Frames;

        private double[] LinkInSignal(IList<Frame> frames) {
            int signalSize = frames[0].Bounds.Width * frames.Count;
            double[] singal = new double[signalSize];
            var orderedFrames = frames.OrderBy(frame => frame.Bounds.Start);
            int currentIndex = 0;
            foreach(Frame frame in orderedFrames) {
                frame.Signal.CopyTo(singal, currentIndex);
                currentIndex += frame.Bounds.Width;
            }
            return singal;
        }
             
        private List<Frame> SplitIntoFrames(double[] speechSignal, int frameWidth) {
            int equalization = speechSignal.Length % frameWidth == 0 ? 0 : 1;
            int frameCount = speechSignal.Length / frameWidth + equalization;
            var frames = new List<Frame>();
            for (int i = 0; i < frameCount; i++) {
                int frameStart = i * frameWidth;
                int frameEnd = GetFrameEnd(i, frameWidth, speechSignal.Length);
                var frameBounds = new SpeechBounds(frameStart, frameEnd);
                double[] frameSingal = new double[frameBounds.Width];
                Array.Copy(speechSignal, frameStart, frameSingal, 0, frameSingal.Length);
                var frame = new Frame(frameSingal, frameBounds);
                frames.Add(frame);
            }
            return frames;
        }

        private int GetFrameEnd(int frameNumber, int frameWidth, int signalLength) {
            int precalculatedEnd = (frameNumber + 1) * frameWidth - 1;
            return precalculatedEnd > signalLength ? signalLength - 1 : precalculatedEnd;
        }

        public SignalContainer Copy() {
            return new SignalContainer(this);
        }
    }
}
