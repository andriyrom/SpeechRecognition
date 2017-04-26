using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.Core {
    public class SimpleSpeechDetector : ISpeechDetector {
        public SignalContainer GetSpeech(SignalContainer rawSignal) {
            SignalContainer singal = rawSignal.Copy();
            singal.FrameSize = 100; //ToDo! Constant change 
            List<double> framesEnergy = singal.GetFrames().Select(frame => SignalHelper.GetAverageEnergy(frame.Signal)).ToList();
            double averageEnergy = framesEnergy.Average();
            double[] flattenedFramesEnergy = ApplyMeanFilter(framesEnergy.ToArray(), 3);
            SpeechBounds speechFramesBounds = GetSpeechBounds(flattenedFramesEnergy, averageEnergy);
            var speechFrames = singal.GetFrames().Skip(speechFramesBounds.Start + 1).Take(speechFramesBounds.Width);
            double[] speechSignal = speechFrames.SelectMany(frame => frame.Signal).ToArray(); 
            return new SignalContainer(speechSignal) {
                FrameSize = rawSignal.FrameSize,
                FrameShift = rawSignal.FrameShift
            };
        }

        private SpeechBounds GetSpeechBounds(double[] framesEnergy, double averageEnergy) {
            int lowerFrameIndex = 0;
            int upperFrameIndex = framesEnergy.Length - 1;
            while (lowerFrameIndex < framesEnergy.Length
                && framesEnergy[lowerFrameIndex] < averageEnergy) { lowerFrameIndex++; }
            while (upperFrameIndex > lowerFrameIndex 
                && framesEnergy[upperFrameIndex] < averageEnergy) { upperFrameIndex--; }
            return new SpeechBounds(lowerFrameIndex, upperFrameIndex);
        }

        private double[] ApplyMeanFilter(double[] signal, int order=3) {
            double[] processed = new double[signal.Length];
            for (int i = 0; i < signal.Length; i++) {
                processed[i] = Mean(signal, i, order);
            }
            return processed;
        }

        private double Mean(double[] signal, int index, int order) {
            int halfOrder = order / 2;
            double sum = 0;
            for (int i = index - halfOrder; i <= index + halfOrder; i++) {
                sum += GetElement(signal, i);
            }
            return sum / order;
        }

        private double GetElement(double[] signal, int index) {
            if (index < 0) { return signal.First(); }
            if (index > signal.Length - 1) { return signal.Last(); }
            return signal[index];
        }
    }
}
