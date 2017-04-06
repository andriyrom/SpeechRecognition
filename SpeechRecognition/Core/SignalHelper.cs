using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeechRecognition.Core {
    public static class SignalHelper {
        public static double GetEnergy(double[] signal, int firstSample, int lastSample) {
            double result = 0;
            double result2 = signal.Skip(firstSample).Take(lastSample - firstSample + 1).Sum(element => Math.Pow(element, 2));
            for (int i = firstSample; i <= lastSample; i++) {
                result += Math.Pow(signal[i], 2);
            }
            return result;
        }

        public static double GetAverageEnergy(double[] signal) {
            return GetAverageEnergy(signal, 0, signal.Length -1);
        }

        public static double GetAverageEnergy(double[] signal, int firstSample, int lastSample) {
            double energy = GetEnergy(signal, firstSample, lastSample);
            int signalWith = lastSample - firstSample + 1;
            return energy / signalWith;
        }

        public static int GetZeroCrossingRate(double[] signal, int firstSample, int lastSample) {
            int crossingCount = 0;
            var signalPart = signal.Skip(firstSample).TakeWhile((element, index) => index <= lastSample - firstSample);
            double average = signalPart.Average();
            for (int i = firstSample; i < lastSample; i++) {
                crossingCount += Math.Sign(signal[i] - average) != Math.Sign(signal[i + 1] - average) ? 1 : 0;
            }
            return crossingCount;
        }

        public static List<FrameFeatures> GetFrameFeatures(double[] speechSignal, int frameWidth) {
            var container = new SignalContainer(speechSignal) {
                FrameSize = frameWidth
            };
            IList<Frame> frames = container.GetFrames();
            return frames.Select(frame => new FrameFeatures(frame)).ToList();
        }     
    }
}
