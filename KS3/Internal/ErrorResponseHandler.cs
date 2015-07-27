using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

using KS3.Transform;
using KS3.Http;
using KS3.KS3Exception;

namespace KS3.Internal
{
    public class ErrorResponseHandler : HttpResponseHandler<ServiceException>
    {
        /** The SAX unmarshaller to use when handling the response from KS3 */
        private ErrorResponseUnmarshaller unmarshaller;

        public ErrorResponseHandler(ErrorResponseUnmarshaller unmarshaller)
        {
            this.unmarshaller = unmarshaller;
        }


        public ServiceException handle(HttpWebResponse errorResponse)
        {
            ServiceException serviceException = unmarshaller.unmarshall(errorResponse.GetResponseStream());
            serviceException.setStatusCode((int)errorResponse.StatusCode);
            return serviceException;
        }
    }
}
