using System.Net;

namespace KS3.Model
{
    public class PutAdpResult
    {
        public HttpStatusCode Status { get; set; }

        public string TaskId { get; set; }
    }
}
