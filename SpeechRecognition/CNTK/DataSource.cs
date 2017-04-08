using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.CNTK {
    public class DataSource : IDataSource {
        private IEnumerator<ISample> Iterator;

        private List<ISample> Samples;

        public DataSource(string name, IList<ISample> samples) {
            Name = name;
            Samples = new List<ISample>(samples);
            Iterator = Samples.GetEnumerator();
        }

        public string Name { get; private set; }

        public ISample NextSample() {
            return Iterator.MoveNext() ? Iterator.Current : null;
        }

        public void Reset() {
            Iterator.Reset();
        }
    }
}
