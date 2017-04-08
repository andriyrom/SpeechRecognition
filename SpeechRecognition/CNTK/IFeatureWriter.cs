using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.CNTK {
    public interface IFeatureWriter {
        void WriteToFile(DataContainer data, String filePath);

        void WriteToStream(DataContainer data, Stream streamWritter);
    }
}
