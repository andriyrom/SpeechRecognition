using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace SpeechRecognition.Core {
    public static class WavReader {
        public static double[] Read(String path, out int sampleRate) {
            WaveFileReader waveReader = new WaveFileReader(path);
            sampleRate = waveReader.WaveFormat.SampleRate;
            byte[] channelBytes = new byte[waveReader.Length];
            waveReader.Read(channelBytes, 0, channelBytes.Length);
            waveReader.Close();
            if (waveReader.WaveFormat.Encoding == WaveFormatEncoding.IeeeFloat) {
                return ConvertIeeeFloat(channelBytes, waveReader.WaveFormat, waveReader.SampleCount);
            } else if (waveReader.WaveFormat.Encoding == WaveFormatEncoding.Pcm) {
                return ConvertPCM(channelBytes, waveReader.WaveFormat, waveReader.SampleCount);
            } else {
                return new double[0];
            }
        }

        private static double[] ConvertIeeeFloat(byte[] rawWave, WaveFormat format, long sampleCount) {
            WaveBuffer waveBuf = new WaveBuffer(rawWave);
            float[] flBuf = waveBuf.FloatBuffer;
            double[] res = new double[sampleCount];
            for (int i = 0; i < res.Length; i++) {
                res[i] = flBuf[format.Channels * i];
            }
            return res;
        }

        private static double[] ConvertPCM(byte[] rawWave, WaveFormat format, long sampleCount) {
            double[] res = new double[sampleCount];
            for (int i = 0; i < res.Length; i++) {
                int temp = BitConverter.ToInt16(rawWave, i * 2 * format.Channels);
                res[i] = ((double)temp) / short.MaxValue;
            }
            double maxValue = res.Max(number => Math.Abs(number));
            res = res.Select(number => number / maxValue).ToArray();
            return res;
        }
    }
}
