// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using WcfSoapLogger.Exceptions;

namespace WcfSoapLogger.FileWriting
{
    public class SoapMessage
    {
        public bool Request { get; private set; }
        public XDocument XmlDoc { get; private set; }

        private SoapMessage()
        {
        }

        public static SoapMessage Parse(byte[] body, bool request)
        {
            var message = new SoapMessage();
            message.Request = request;

            try
            {
                using (var reader = XmlReader.Create(new MemoryStream(body)))
                {
                    message.XmlDoc = XDocument.Load(reader);
                }
            }
            catch (Exception ex)
            {
                throw new LoggerException("Given byte array is not a valid XML document.", ex);
            }

            return message;
        }


        public string GetOperationName()
        {
            const string Response = "Response";
            string operation = "OperationNameNotFound";
            XElement nodeBody = FindNodeByPath("Body");

            if (nodeBody != null && nodeBody.HasElements)
            {
                operation = nodeBody.Elements().First().Name.LocalName;

                if (operation.EndsWith(Response))
                {
                    operation = operation.Substring(0, operation.Length - Response.Length);
                }
            }

            return operation;
        }

        public string GetNodeValue(string path)
        {
          string[] array = path.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
          return GetNodeValueByArray(array);
        }

        public string GetNodeValueByArray(params string[] array)
        {
            XElement node = FindNodeByPath(array);

            if (node == null)
            {
                return null;
            }

            return node.Value;
        }


        //we don't want to care about element namespaces, we need only LocalNames
        public XElement FindNodeByPath(params string[] array) 
        {
            int index = 0;
            XElement node = XmlDoc.Root;

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


        private static XElement FindNodeByName(IEnumerable<XElement> list, string name) 
        {
            foreach (XElement node in list)
            {
                if (string.Equals(node.Name.LocalName, name.Trim(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return node;
                }
            }

            return null;
        }


        public string GetIndentedXml() 
        {
            var sb = new StringBuilder();
            var xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            xmlSettings.IndentChars = "  ";
            xmlSettings.NewLineChars = "\r\n";
            xmlSettings.NewLineHandling = NewLineHandling.Replace;

            using (XmlWriter writer = XmlWriter.Create(sb, xmlSettings))
            {
                XmlDoc.Save(writer);
            }

            string xml = sb.ToString();
            return xml;
        }
    }
}
