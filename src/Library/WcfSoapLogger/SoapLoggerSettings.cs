// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace WcfSoapLogger
{
    public class SoapLoggerSettings
    {
        public string LogPath { get; internal set; }
        public bool UseCustomHandler { get; internal set; }

        internal bool IsService { get; set; }
        internal bool IsClient { get { return !IsService; } }
    }
}
