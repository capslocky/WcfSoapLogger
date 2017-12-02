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
