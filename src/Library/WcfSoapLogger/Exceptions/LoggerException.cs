// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;

namespace WcfSoapLogger.Exceptions
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
