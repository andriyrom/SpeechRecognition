using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.Core {
    public class SignalContainer {
        private bool ShouldRebuildFrames;

        private List<Frame> Frames;

        public double[] Signal { 
            get {
                return CopySignal(vSignal);
            }
        }
        private double[] vSignal;

        public int FrameSize { 
            get {
                return vFrameSize;
            } 
            set {
                vFrameSize = value;
                ShouldRebuildFrames = true;
            } 
        }
        private int vFrameSize;

        public int FrameShift {
            get {
                return vFrameShift;
            }
            set {
                vFrameShift = value;
                ShouldRebuildFrames = true;
            }
        }
        private int vFrameShift;

        private SignalContainer(SignalContainer other) {
            FrameSize = other.FrameSize;
            FrameShift = other.FrameShift;
            vSignal = other.Signal;
            ShouldRebuildFrames = other.ShouldRebuildFrames;
            Frames = other.Frames == null ? null : new List<Frame>(other.Frames);
        }

        public SignalContainer(double[] signal) {
            vSignal = CopySignal(signal);
            FrameSize = vSignal.Length;
            FrameShift = FrameSize;
            ShouldRebuildFrames = true;
        }

        private double[] CopySignal(double[] signal) {
            var copiedSignal = new double[signal.Length];
            signal.CopyTo(copiedSignal, 0);
            return copiedSignal;
        }

        public List<Frame> GetFrames() {
            if (Frames == null || ShouldRebuildFrames) {
                Frames = SplitIntoFrames(vSignal, FrameSize, FrameShift);
                ShouldRebuildFrames = false;
            }
            return new List<Frame>(Frames);
        }        

        private List<Frame> SplitIntoFrames(double[] speechSignal, int frameWidth, int frameShift) {
            int frameCount = GetFramesCount(speechSignal.Length, frameWidth, frameShift);
            var frames = new List<Frame>();
            for (int i = 0; i < frameCount; i++) {
                int frameStart = i * frameShift;
                int frameEnd = GetFrameEnd(i, frameWidth, frameShift, speechSignal.Length);
                var frameBounds = new SpeechBounds(frameStart, frameEnd);
                double[] frameSingal = new double[frameBounds.Width];
                Array.Copy(speechSignal, frameStart, frameSingal, 0, frameSingal.Length);
                var frame = new Frame(frameSingal, frameBounds);
                frames.Add(frame);
            }
            return frames;
        }

        private int GetFramesCount(int signalLength, int frameWidth, int frameShift) {
            int frameCount = 1;
            int signalUnderShiftedFrames = signalLength - frameWidth;
            if (signalUnderShiftedFrames > 0) {
                int equalization = signalUnderShiftedFrames % frameShift == 0 ? 0 : 1;
                frameCount += signalUnderShiftedFrames / frameShift + equalization;
            }             
            return frameCount;
        }

        private int GetFrameEnd(int frameNumber, int frameWidth, int frameShift, int signalLength) {
            int precalculatedEnd = frameNumber * frameShift + frameWidth - 1;
            return precalculatedEnd > signalLength ? signalLength - 1 : precalculatedEnd;
        }

        public SignalContainer Copy() {
            return new SignalContainer(this);
        }
    }
}
