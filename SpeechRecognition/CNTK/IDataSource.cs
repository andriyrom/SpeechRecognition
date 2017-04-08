using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.CNTK {
    public interface IDataSource {
        string Name { get; }

        ISample NextSample();

        void Reset();
    }
}
