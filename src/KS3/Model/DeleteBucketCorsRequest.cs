namespace KS3.Model
{
    public class DeleteBucketCorsRequest : KS3Request
    {

        public string BucketName { get; set; }

        public DeleteBucketCorsRequest()
        {
        }

        public DeleteBucketCorsRequest(string bucketName)
        {
            BucketName = bucketName;
        }

    }
}
