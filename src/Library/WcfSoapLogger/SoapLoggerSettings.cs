using System;

namespace WcfSoapLogger
{
    public class SoapLoggerSettings
    {
        public string LogPath { get; internal set; }

        internal bool IsService { get; set; }
        internal bool IsClient { get { return !IsService; } }
    }
}
