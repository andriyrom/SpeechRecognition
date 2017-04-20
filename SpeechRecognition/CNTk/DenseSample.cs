using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.CNTK {
    public class DenseSample : ISample {
        private double[] SampleElements;

        public DenseSample(double[] data) {
            SampleElements = new double[data.Length];
            data.CopyTo(SampleElements, 0);
        }

        public string GetString() {
            StringBuilder sampleElementsString = new StringBuilder(); 
            foreach(double element in SampleElements) {
                sampleElementsString.Append(string.Format(CultureInfo.InvariantCulture, "{0} ", element));
            }
            return sampleElementsString.ToString();
        }

        public byte[] GetBytes() {
            throw new NotImplementedException();
        }
    }
}
