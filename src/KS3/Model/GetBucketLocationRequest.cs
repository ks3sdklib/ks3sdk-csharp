namespace KS3.Model
{
    public class GetBucketLocationRequest : KS3Request
    {

        public string BucketName { get; set; }

        public GetBucketLocationRequest()
        {

        }

        public GetBucketLocationRequest(string bucketName)
        {
            BucketName = bucketName;
        }
    }
}
