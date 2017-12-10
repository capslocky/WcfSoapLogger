// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.IO;
using WcfSoapLogger.Exceptions;

namespace WcfSoapLogger.FileWriting
{
    public static class SoapLoggerTools
    {

        public static void WriteFile(FileName fileName, string text, byte[] bytes, string logPath)
        {
            string folder = Path.Combine(logPath, fileName.DateTime.ToString("yyyy-MM-dd"));
            FileSystem.CreateDirectory(folder);

            string filePath = Path.Combine(folder, fileName.Value);

            if (text != null)
            {
                FileSystem.WriteAllText(filePath, text);
                return;
            }

            if (bytes != null)
            {
                FileSystem.WriteAllBytes(filePath, bytes);
                return;
            }

            throw new ArgumentException("No file content provided.");
        }


       

        public static void WriteFileDefault(byte[] body, bool request, string logPath)
        {
            var fileNameFactory = new FileNameFactory();

            try
            {
                var message = SoapMessage.Parse(body, request);
                fileNameFactory.AddSegment(message.GetOperationName());
                fileNameFactory.AddDirection(request);

                string indentedXml = message.GetIndentedXml();

                WriteFile(fileNameFactory.GetFileName(), indentedXml, null, logPath);
            }
            catch (FileSystemAcccesDeniedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                fileNameFactory.AddSegment("ERROR");
                WriteFile(fileNameFactory.GetFileName(), null, body, logPath);

                fileNameFactory.AddSegment("exception");
                WriteFile(fileNameFactory.GetFileName(), ex.ToString(), null, logPath);
            }
        }



    
    }
}
