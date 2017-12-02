using System;
using System.Collections.Generic;

namespace WcfSoapLogger.FileWriting
{
    public class FileNameFactory
    {
        public List<string> Segments { get; private set; }

        public FileNameFactory() 
        {
            Segments = new List<string>();
        }

        public void AddSegment(string segment) 
        {
            if (string.IsNullOrEmpty(segment))
            {
                return;
            }

            //TODO check for invalid chars
            Segments.Add(segment);
        }



        public void AddDirection(bool request) {
            if (request)
            {
                AddSegment("Request");
            }
            else
            {
                AddSegment("Response");
            }
        }


        public FileName GetFileName() 
        {
            string key = String.Join("_", Segments);
            TimeIndex timeIndex = TimeIndex.GetUnique(key);

            string dateTimeText = GetDateTimeText(timeIndex.DateTime);
            string value = dateTimeText + "__" + key;

            if (timeIndex.Index > 0)
            {
                value += "_" + timeIndex.Index;
            }

            value += ".xml";

            var fileName = new FileName(value, timeIndex.DateTime);
            return fileName;
        }


        public static string GetDateTimeText(DateTime dateTime) 
        {
            return dateTime.ToString("yyyy-MM-dd_'at'_HH-mm-ss-fff");
        }
    }
}
