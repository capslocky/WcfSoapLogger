using System;
using System.Diagnostics;
using System.IO;

namespace Service
{
    public class TauTraceListener : TraceListener
    {
        private const string LogFile = @"C:\SoapLog\Tau\Service\TauTraceListener.xml";

        public override void Write(string message)
        {
            File.AppendAllText(LogFile, message + Environment.NewLine + Environment.NewLine);
        }

        public override void WriteLine(string message)
        {
            File.AppendAllText(LogFile, message + Environment.NewLine + Environment.NewLine);
        }
    }
}
