namespace KS3.Model
{
    /// <summary>
    /// Request object containing all the options for requesting a bucket's Access Control List (ACL).
    /// </summary>
    public class GetBucketAclRequest : KS3Request
    {
        /// <summary>
        /// The name of the bucket whose ACL is being retrieved.
        /// </summary>
        public string BucketName { get; set; }

        public GetBucketAclRequest()
        {

        }

        public GetBucketAclRequest(string bucketName)
        {
            BucketName = bucketName;
        }
    }
}
