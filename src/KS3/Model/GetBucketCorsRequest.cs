namespace KS3.Model
{
    public class GetBucketCorsRequest : KS3Request
    {
        public string BucketName { get; set; }

        public GetBucketCorsRequest()
        {

        }

        public GetBucketCorsRequest(string bucketName)
        {
            BucketName = bucketName;
        }
    }
}
