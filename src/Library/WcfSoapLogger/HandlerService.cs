using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfSoapLogger
{
    internal class HandlerService :  HandlerAbstract
    {
        public HandlerService(SoapLoggerSettings settings) : base(settings) {
        }

        protected override void HandleRequest(byte[] body)
        {
            SoapLoggerForService.SetRequestBody(body, settings);
        }

        protected override void HandleResponse(byte[] body)
        {
            SoapLoggerForService.CallResponseCallback(body, settings);
        }

        protected override byte[] GetRequestErrorBody(Exception ex)
        {
            //this can happen only with default handler
            //typically when lacking access to file system 
            SoapLoggerForService.SetRequestException(ex);

            //in order to force HTTP 500 Internal Server Error and avoid processing
            //we overwrite request with deliberately invalid body
            return ErrorBody.GetSoapInvalidRequest();
        }

        protected override byte[] GetResponseErrorBody(Exception ex)
        {
            string message = null;

            if (ex is LoggerException)
            {
                message = ex.Message;
            }
            else
            {
                message = ex.ToString();
            }

            string soapFault = ErrorBody.GetSoapFault(message);
            byte[] errorBody = Encoding.UTF8.GetBytes(soapFault);
            return errorBody;
        }
    }
}
