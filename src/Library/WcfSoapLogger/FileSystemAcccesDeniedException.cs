using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfSoapLogger
{
    public class FileSystemAcccesDeniedException : Exception
    {
        public FileSystemAcccesDeniedException() 
        {
        }

        public FileSystemAcccesDeniedException(string message): base(message)
        {
        }

        public FileSystemAcccesDeniedException(string message, Exception inner): base(message, inner)
        {
        }
    }
}
