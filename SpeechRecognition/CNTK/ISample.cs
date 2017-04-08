using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.CNTK {
    public interface ISample {
        string GetString();
        byte[] GetBytes();
    }
}
