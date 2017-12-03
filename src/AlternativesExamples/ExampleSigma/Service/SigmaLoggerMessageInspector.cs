using System;
using System.IO;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Service
{
    public class SigmaLoggerMessageInspector : IDispatchMessageInspector, IEndpointBehavior
    {
        public object AfterReceiveRequest(ref Message message, IClientChannel channel, InstanceContext instanceContext) {
            Console.WriteLine("AfterReceiveRequest");

            MessageBuffer buffer = message.CreateBufferedCopy(int.MaxValue);
            message = buffer.CreateMessage();

            Message messageCopy = buffer.CreateMessage();
            string contents;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                XmlWriter writer = XmlWriter.Create(memoryStream);
                messageCopy.WriteMessage(writer);
                writer.Flush();

                memoryStream.Position = 0;
                contents = new StreamReader(memoryStream).ReadToEnd();
            }

            LogFile(contents, "Request");
            return null;
        }

        public object AfterReceiveRequest_2(ref Message message, IClientChannel channel, InstanceContext instanceContext) {
            Console.WriteLine("AfterReceiveRequest");

            MessageBuffer buffer = message.CreateBufferedCopy(int.MaxValue);
            message = buffer.CreateMessage();

            Message messageCopy = buffer.CreateMessage();
            string contentsWithoutBody = messageCopy.ToString();

            XmlDictionaryReader reader = messageCopy.GetReaderAtBodyContents();
            string body = reader.ReadOuterXml();
            string contents = contentsWithoutBody.Replace("... stream ...", body);

            LogFile(contents, "Request");
            return null;
        }


        public void BeforeSendReply(ref Message message, object correlationState) {
            Console.WriteLine("BeforeSendReply");

            MessageBuffer buffer = message.CreateBufferedCopy(int.MaxValue);
            message = buffer.CreateMessage();

            Message messageCopy = buffer.CreateMessage();
            string contents = messageCopy.ToString();
            LogFile(contents, "Response");
        }



        private const string LogFolder = @"C:\SoapLog\Sigma\Service";

        private void LogFile(string contents, string name) 
        {
            XDocument xml = XDocument.Parse(contents);
            contents = GetIndentedXml(xml);

            string filename = GetDateTimeText(DateTime.Now) + "_" + Guid.NewGuid() + "__" + name + ".xml";

            Directory.CreateDirectory(LogFolder);
            filename = Path.Combine(LogFolder, filename);
            File.WriteAllText(filename, contents);
        }


        private static string GetDateTimeText(DateTime dateTime) {
            return dateTime.ToString("yyyy-MM-dd_'at'_HH-mm-ss-fff");
        }

        public string GetIndentedXml(XDocument xmlDoc) {
            var sb = new StringBuilder();
            var xmlSettings = new XmlWriterSettings();
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



        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher){
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(this);
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime) {
            throw new Exception("This behavior not supported on the client side.");
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) {
        }

        public void Validate(ServiceEndpoint endpoint) {
        }
    }
}