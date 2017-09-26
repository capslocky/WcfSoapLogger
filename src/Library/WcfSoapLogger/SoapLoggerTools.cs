using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            fileName += ".xml";

            string folder = Path.Combine(logPath, DateTime.Now.ToString("yyyy-MM-dd"));
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


        public static string GetRequestId(XmlDocument xmlDoc) {
            XmlNode node = FindNodeByPath(xmlDoc, "Envelope", "Body", "SendProduct", "metadata", "RequestMessageId");

            if (node == null)
            {
                node = FindNodeByPath(xmlDoc, "Envelope", "Body", "SendProductResponse", "SendProductResult", "RequestMessageId");
            }

            if (node == null)
            {
                node = FindNodeByPath(xmlDoc, "Envelope", "Body", "SendClustersData", "metadata", "RequestMessageId");
            }

            if (node == null)
            {
                node = FindNodeByPath(xmlDoc, "Envelope", "Body", "SendClustersDataResponse", "SendClustersDataResult", "RequestMessageId");
            }

            if (node == null)
            {
                return null;
            }

            return node.InnerText;
        }


        public static string GetProductIdentifier(XmlDocument xmlDoc) {
            XmlNode node = FindNodeByPath(xmlDoc, "Envelope", "Body", "SendProduct", "product", "ProductIdentifier");

            if (node == null)
            {
                return null;
            }

            return node.InnerText;
        }

        public static void LogBytes(byte[] bytes, bool response, string logPath) {
            var thread = Thread.CurrentThread;
            //            string text = new UTF8Encoding().GetString(incomingBytes);

            var xmlDoc = new XmlDocument();
            var fileName = new StringBuilder();

            AddFileNamePart(fileName, GetDateTimeText());

            try
            {
                using (var reader = XmlReader.Create(new MemoryStream(bytes)))
                {
                    xmlDoc.Load(reader);
                }

                AddFileNamePart(fileName, GetMethodName(xmlDoc));
                AddFileNamePart(fileName, GetRequestId(xmlDoc));

                if (response)
                {
                    AddFileNamePart(fileName, Response);
                }
                else
                {
                    AddFileNamePart(fileName, GetProductIdentifier(xmlDoc));
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







        public static string GetDateTimeText() {
            DateTime dateTime = DateTime.Now;

            string text = dateTime.ToString("yyyy-MM-dd") + " at " + dateTime.ToString("HH-mm-ss-fff");

            //            fileName += "@ " + methodName;
            //
            //            int ordinal = GetConcurrentOrdinal(methodName, dateTime);
            //
            //            if (ordinal > 0)
            //            {
            //                fileName += "_" + ordinal.ToString("00");
            //            }

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

//        internal Dictionary<string, DateTime> dicDate = new Dictionary<string, DateTime>();
//        internal Dictionary<string, int> dicCount = new Dictionary<string, int>();
//
//        private int GetConcurrentOrdinal(string method, DateTime dateTime) {
//            if (!dicDate.ContainsKey(method))
//            {
//                dicDate.Add(method, new DateTime(2000, 1, 1));
//            }
//
//            if (!dicCount.ContainsKey(method))
//            {
//                dicCount.Add(method, 0);
//            }
//
//            DateTime lastDate = dicDate[method];
//
//            if (lastDate == dateTime)
//            {
//                dicCount[method] = dicCount[method] + 1;
//                return dicCount[method];
//            }
//
//            dicDate[method] = dateTime;
//            dicCount[method] = 0;
//
//            return 0;
//        }


        private static string GetMethodName(XmlDocument xml) {
            string result = "SoapMethodNameNotFound";
            XmlNode nodeBody = FindNodeByPath(xml, "Envelope", "Body");

            if (nodeBody != null && nodeBody.ChildNodes.Count > 0)
            {
                result = nodeBody.ChildNodes[0].LocalName;
            }

            return result;
        }


        private static XmlNode FindNodeByPath(XmlDocument xmlDoc, params string[] array) {
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
