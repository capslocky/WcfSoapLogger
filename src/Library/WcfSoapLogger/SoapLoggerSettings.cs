using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfSoapLogger
{
    public class SoapLoggerSettings
    {
        public string LogPath { get; internal set; }
        public bool IsService { get; internal set; }
        public bool IsClient { get { return !IsService; } }

    }
}
