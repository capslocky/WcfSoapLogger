using System;

namespace WcfSoapLogger
{
    public class LoggerException : Exception
    {
        public LoggerException() {
        }

        public LoggerException(string message): base(message)
        {
        }

        public LoggerException(string message, Exception inner): base(message, inner)
        {
        }
    }
}
