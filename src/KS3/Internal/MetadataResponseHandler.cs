using KS3.Http;
using KS3.Model;
using System.Net;

namespace KS3.Internal
{
    /// <summary>
    /// KS3 response handler that knows how to pull KS3 object metadata out of a response and unmarshall it into an ObjectMetadata object.
    /// </summary>
    public class MetadataResponseHandler : IHttpResponseHandler<ObjectMetadata>
    {
        public ObjectMetadata Handle(HttpWebResponse response)
        {
            ObjectMetadata metadata = new ObjectMetadata();
            RestUtils.PopulateObjectMetadata(response, metadata);

            return metadata;
        }
    }
}
