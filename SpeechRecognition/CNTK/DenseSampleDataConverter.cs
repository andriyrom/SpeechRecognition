using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.CNTK {
    public class DenseSampleDataConverter {
        private List<IDataSource> DataSources = new List<IDataSource>();

        public void AddDataSource(string name, IList<double[]> samples) {
            IList<ISample> dataSamples = samples.Select(sample => new DenseSample(sample) as ISample).ToList();
            DataSource dataSource = new DataSource(name, dataSamples);
        }

        public DataContainer GetDataContainer() {
            return new DataContainer(DataSources);
        }

    }
}
