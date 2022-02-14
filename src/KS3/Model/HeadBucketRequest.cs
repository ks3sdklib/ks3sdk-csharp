namespace KS3.Model
{
    public class HeadBucketRequest : KS3Request
    {
        /// <summary>
        /// The name of the bucket whose ACL is being retrieved. 
        /// </summary>
        public string BucketName { get; set; }
        public HeadBucketRequest()
        {

        }

        public HeadBucketRequest(string bucketName)
        {
            BucketName = bucketName;
        }
    }
}
