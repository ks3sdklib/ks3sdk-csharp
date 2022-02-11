namespace KS3.Model
{
    public class GetBucketLoggingRequest : KS3Request
    {
        public string BucketName { get; set; }

        public GetBucketLoggingRequest()
        {

        }

        public GetBucketLoggingRequest(string bucketName)
        {
            BucketName = bucketName;
        }
    }
}
