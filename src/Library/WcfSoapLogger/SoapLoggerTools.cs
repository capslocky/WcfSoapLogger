using System;
using System.IO;
using System.Text;
using System.Xml;

namespace WcfSoapLogger
{
    public static class SoapLoggerTools
    {
        private const string Response = "Response";

        public static void AddFileNamePart(StringBuilder fileName, string value) {
            if (value == null)
            {
                return;
            }

            if (value.Length > Response.Length && value.EndsWith(Response))
            {
                //                value = value.Substring(0, value.Length - Response.Length) + " @ " + Response;
                value = value.Substring(0, value.Length - Response.Length);
            }

            fileName.Append(value).Append(" @ ");
        }

        public static void WriteFile(string fileName, string text, byte[] bytes, bool error, string logPath) {
            fileName = fileName.Remove(fileName.Length - 3, 3);

            var timeIndex = TimeIndex.GetUnique(fileName);

            string dateTimeText = GetDateTimeText(timeIndex.DateTime);
            fileName = dateTimeText + " @ " + fileName;

            if (timeIndex.Index > 0)
            {
                fileName += "_" + timeIndex.Index.ToString("00");
            }

            fileName += ".xml";

            string folder = Path.Combine(logPath, timeIndex.DateTime.ToString("yyyy-MM-dd"));
            Directory.CreateDirectory(folder);
            string filePath = Path.Combine(folder, fileName);

            if (text != null)
            {
                File.WriteAllText(filePath, text);
                return;
            }

            if (bytes != null)
            {
                File.WriteAllBytes(filePath, bytes);
                return;
            }

            throw new ArgumentException("No file data provided.");
        }


       

        public static void LogBytes(byte[] bytes, bool request, string logPath) {
            //            string text = new UTF8Encoding().GetString(incomingBytes);

//            throw new InvalidOperationException("oops!");


            var xmlDoc = new XmlDocument();
            var fileName = new StringBuilder();

            try
            {
                using (var reader = XmlReader.Create(new MemoryStream(bytes)))
                {
                    xmlDoc.Load(reader);
                }

                AddFileNamePart(fileName, GetMethodName(xmlDoc));

                if (request)
                {
                }
                else
                {
                    AddFileNamePart(fileName, Response);
                }

                string indentedXml = GetIndentedXml(xmlDoc);

                WriteFile(fileName.ToString(), indentedXml, null, false, logPath);
            } catch (Exception ex)
            {
                AddFileNamePart(fileName, "ERROR non-xml");
                WriteFile(fileName.ToString(), null, bytes, true, logPath);

                AddFileNamePart(fileName, "exception");
                WriteFile(fileName.ToString(), ex.ToString(), null, true, logPath);
            }
        }







        public static string GetDateTimeText(DateTime dateTime) {
            string text = dateTime.ToString("yyyy-MM-dd") + " at " + dateTime.ToString("HH-mm-ss-fff");
            return text;
        }



        private static string GetIndentedXml(XmlDocument xmlDoc) {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            xmlSettings.IndentChars = "  ";
            xmlSettings.NewLineChars = "\r\n";
            xmlSettings.NewLineHandling = NewLineHandling.Replace;

            using (XmlWriter writer = XmlWriter.Create(sb, xmlSettings))
            {
                xmlDoc.Save(writer);
            }

            string xml = sb.ToString();
            return xml;
        }




        private static string GetMethodName(XmlDocument xml) {
            string result = "SoapMethodNameNotFound";
            XmlNode nodeBody = FindNodeByPath(xml, "Envelope", "Body");

            if (nodeBody != null && nodeBody.ChildNodes.Count > 0)
            {
                result = nodeBody.ChildNodes[0].LocalName;
            }

            return result;
        }


        public static XmlNode FindNodeByPath(XmlDocument xmlDoc, params string[] array) {
            int index = 0;
            XmlNode node = xmlDoc;

            while (index < array.Length)
            {
                node = FindNodeByName(node.ChildNodes, array[index]);

                if (node == null)
                {
                    return null;
                }

                index++;
            }

            return node;
        }


        private static XmlNode FindNodeByName(XmlNodeList list, string name) {
            foreach (XmlNode node in list)
            {
                if (string.Equals(node.LocalName, name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return node;
                }
            }

            return null;
        }
    }
}
