using KS3.Http;
using KS3.KS3Exception;
using KS3.Transform;
using System.Net;

namespace KS3.Internal
{
    public class ErrorResponseHandler : IHttpResponseHandler<ServiceException>
    {
        /// <summary>
        /// The SAX unmarshaller to use when handling the response from KS3
        /// </summary>
        private readonly ErrorResponseUnmarshaller _unmarshaller;

        public ErrorResponseHandler(ErrorResponseUnmarshaller unmarshaller)
        {
            _unmarshaller = unmarshaller;
        }


        public ServiceException Handle(HttpWebResponse errorResponse)
        {
            ServiceException serviceException = _unmarshaller.Unmarshall(errorResponse.GetResponseStream());
            serviceException.StatusCode = (int)errorResponse.StatusCode;
            return serviceException;
        }
    }
}
