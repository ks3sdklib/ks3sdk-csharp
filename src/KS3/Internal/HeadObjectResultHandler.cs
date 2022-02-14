using KS3.Http;
using KS3.Model;
using System.Net;

namespace KS3.Internal
{
    public class HeadObjectResultHandler : IHttpResponseHandler<HeadObjectResult>
    {
        public HeadObjectResult Handle(HttpWebResponse response)
        {
            var result = new HeadObjectResult();
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    ConvertObjectMetaData(result, response);
                    break;
                case HttpStatusCode.PartialContent:
                    ConvertObjectMetaData(result, response);
                    break;
                case HttpStatusCode.NotModified:
                    result.IfModified = false;
                    break;
                case HttpStatusCode.PreconditionFailed:
                    result.IfPreconditionSuccess = false;
                    break;
                default:
                    break;
            }
            return result;
        }
        private void ConvertObjectMetaData(HeadObjectResult result, HttpWebResponse response)
        {
            ObjectMetadata metadata = new ObjectMetadata();
            RestUtils.PopulateObjectMetadata(response, metadata);
            result.ObjectMetadata = metadata;
        }
    }
}
