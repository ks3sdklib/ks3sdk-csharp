using KS3.Http;
using KS3.Model;
using System.Net;

namespace KS3.Internal
{
    public class HeadBucketResponseHandler : IHttpResponseHandler<HeadBucketResult>
    {
        public HeadBucketResult Handle(HttpWebResponse response)
        {
            var result = new HeadBucketResult
            {
                StatueCode = response.StatusCode
            };
            return result;
        }
    }
}
