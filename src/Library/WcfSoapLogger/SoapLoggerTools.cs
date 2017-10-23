using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

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

            fileName.Append(value).Append("_");
        }

        public static void WriteFile(string fileName, string text, byte[] bytes, bool error, string logPath) {
            fileName = fileName.Remove(fileName.Length - 1, 1);

            var timeIndex = TimeIndex.GetUnique(fileName);

            string dateTimeText = GetDateTimeText(timeIndex.DateTime);
            fileName = dateTimeText + "__" + fileName;

            if (timeIndex.Index > 0)
            {
                fileName += "_" + timeIndex.Index.ToString("0");
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


       

        public static void WriteFileDefault(byte[] bytes, bool request, string logPath) {
//            string text = new UTF8Encoding().GetString(bytes);
//            throw new InvalidOperationException("oops!");


            var fileName = new StringBuilder();

            try
            {
                XDocument xmlDoc;

                using (var reader = XmlReader.Create(new MemoryStream(bytes)))
                {
                    xmlDoc = XDocument.Load(reader);
                }


                AddFileNamePart(fileName, GetSoapOperationName(xmlDoc));

                if (request)
                {
                    AddFileNamePart(fileName, "Request");
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







        public static string GetDateTimeText(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd_'at'_HH-mm-ss-fff");
        }



        private static string GetIndentedXml(XDocument xmlDoc) {
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




        private static string GetSoapOperationName(XDocument xml)
        {
            string result = "SoapOperationNameNotFound";
            XElement nodeBody = FindNodeByPath(xml, "Body");

            if (nodeBody != null && nodeBody.HasElements)
            {
                result = nodeBody.Elements().First().Name.LocalName;
            }

            return result;
        }


        //we don't want to care about namespaces
        public static XElement FindNodeByPath(XDocument xmlDoc, params string[] array) {
            int index = 0;
            XElement node = xmlDoc.Root;

            while (index < array.Length)
            {
                node = FindNodeByName(node.Elements(), array[index]);

                if (node == null)
                {
                    return null;
                }

                index++;
            }


            return node;
        }


        private static XElement FindNodeByName(IEnumerable<XElement> list, string name) {
            foreach (XElement node in list)
            {
                if (string.Equals(node.Name.LocalName, name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return node;
                }
            }

            return null;
        }
    }
}
