namespace KS3.Model
{
    /// <summary>
    /// Provides options for creating an KS3 bucket.
    /// </summary>
    public class CreateBucketRequest : KS3Request
    {

        /// <summary>
        /// The name of the KS3 bucket to create.
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// An optional access control list to apply to the new object. If specified, cannedAcl will be ignored.
        /// </summary>
        public AccessControlList Acl { get; set; }

        /// <summary>
        /// The optional Canned ACL to set for the new bucket. Ignored in favor of accessControlList, if present
        /// </summary>
        public CannedAccessControlList CannedAcl { get; set; }

        public CreateBucketRequest(string bucketName)
        {
            BucketName = bucketName;
        }
    }
}
