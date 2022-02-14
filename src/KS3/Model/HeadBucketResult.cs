using System.Net;

namespace KS3.Model
{
    public class HeadBucketResult
    {
        /// <summary>
        /// The operation returns a 200 OK if the bucket exists and you have permission to access it. 
        /// Otherwise, the operation might return responses such as 404 Not Found and 403 Forbidden.  
        /// </summary>
        public HttpStatusCode StatueCode { get; set; }

    }
}
