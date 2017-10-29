using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfSoapLogger
{
    internal class HandlerServiceRequest : Handler
    {
        internal override byte[] HandleBody(SoapLoggerSettings settings, byte[] body)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                string xmlError = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body></s:Body></s:Envelope>";
                byte[] errorBody = Encoding.UTF8.GetBytes(xmlError);
                return errorBody;
            }

            return null;
        }
    }
}
