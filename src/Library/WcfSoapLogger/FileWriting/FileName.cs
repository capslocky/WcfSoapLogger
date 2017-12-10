// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;

namespace WcfSoapLogger.FileWriting
{
    public class FileName
    {
        public string Value { get; private set; }
        public DateTime DateTime { get; private set; }

        public FileName(string value, DateTime dateTime)
        {
            Value = value;
            DateTime = dateTime;
        }
    }
}
