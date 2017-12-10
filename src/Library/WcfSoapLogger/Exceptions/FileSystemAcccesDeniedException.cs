// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Security.Principal;

namespace WcfSoapLogger.Exceptions
{
    public class FileSystemAcccesDeniedException : LoggerException
    {
        private readonly string originalMessage;
        private readonly string currentUserName;

        public FileSystemAcccesDeniedException(UnauthorizedAccessException ex) : base(null, ex)
        {
            //"Access to the path 'C:\SoapLogDefaultService\2017-10-31' is denied."
            originalMessage = ex.Message;

            //never returns null
            currentUserName = WindowsIdentity.GetCurrent().Name;
        }

        public override string Message{
            get{
                return originalMessage + " You need to grant access to the user: " + currentUserName;
            }
        }
    }
}
