using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.CNTK {
    public class CNTKTextFormatWriter : IFeatureWriter {
        private const string FeatureTemplate = "|{0} {1}";

        public void WriteToFile(DataContainer data, string filePath) {
            FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
            WriteToStream(data, fileStream);
            fileStream.Close();
        }

        public void WriteToStream(DataContainer data, Stream stream) {
            IList<IDataSource> dataSources = data.GetDataSources();
            string[] dataSourceNames = data.GetDataSourceNames().ToArray();
            StringBuilder textContent = new StringBuilder();
            List<ISample> currentSamples = GetSamples(dataSources);
            while (currentSamples.Any()) {
                AddFeatureString(textContent, dataSourceNames, currentSamples);
                currentSamples = GetSamples(dataSources);
            }
            ResetDataSources(dataSources);
            WriteContentToStream(textContent.ToString(), stream);
        }

        private List<ISample> GetSamples(IList<IDataSource> dataSources) {
            return dataSources.Select(dataSource => dataSource.NextSample()).Where(sample => sample != null).ToList();
        }

        private void ResetDataSources(IList<IDataSource> dataSources) {
            foreach(IDataSource dataSource in dataSources) {
                dataSource.Reset();
            }
        }

        private void AddFeatureString(StringBuilder textFileContent, string[] dataSourceNames, IList<ISample> currentSamples) {
            for (int i = 0; i < dataSourceNames.Length; i++) {
                string featureString = string.Format(FeatureTemplate, dataSourceNames[i], currentSamples[i].GetString());
                textFileContent.Append(featureString);
            }
            textFileContent.AppendLine();
        }

        private void WriteContentToStream(string content, Stream stream) {
            using (StreamWriter writter = new StreamWriter(stream)) {
                writter.Write(content);
            }
        }
    }
}
