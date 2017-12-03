using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Client
{
    public class SigmaLoggerMessageInspector : IClientMessageInspector, IEndpointBehavior
    {
        public object BeforeSendRequest(ref Message message, IClientChannel channel)
        {
            Console.WriteLine("BeforeSendRequest");

            MessageBuffer buffer = message.CreateBufferedCopy(int.MaxValue);
            message = buffer.CreateMessage();

            Message messageCopy = buffer.CreateMessage();
            string contents = messageCopy.ToString();
            LogFile(contents, "Request");
            return null;
        }

        public void AfterReceiveReply(ref Message message, object correlationState)
        {
            Console.WriteLine("AfterReceiveReply");

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

            LogFile(contents, "Response");
        }


        private Message Intercept(Message message) {
            MemoryStream ms = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(ms);
            message.WriteMessage(writer);
            writer.Flush();
            ms.Position = 0;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(ms);

            // read the contents of the message here and update as required; eg sign the message

            // as the message is forward reading then we need to recreate it before moving on 
            ms = new MemoryStream();
            xmlDoc.Save(ms);
            ms.Position = 0;
            XmlReader reader = XmlReader.Create(ms);
            Message newMessage = Message.CreateMessage(reader, int.MaxValue, message.Version);
            newMessage.Properties.CopyProperties(message.Properties);
            message = newMessage;
            return message;
        }




        private const string LogFolder = @"C:\SoapLog\Sigma\Client";

        private void LogFile(string contents, string name) {
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









        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) {
            throw new Exception("This behavior not supported on the service side.");
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime) {
            clientRuntime.MessageInspectors.Add(this);
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) {
        }

        public void Validate(ServiceEndpoint endpoint) {
        }
    }
}
