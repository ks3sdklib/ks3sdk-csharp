namespace KS3.Model
{
    /// <summary>
    /// Request object containing all the options for setting a bucket's Access Control List (ACL).
    /// </summary>
    public class SetBucketAclRequest : KS3Request
    {
        /// <summary>
        /// Bucket name
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// The custom ACL to apply to the specified bucket
        /// </summary>
        public AccessControlList Acl { get; set; }

        /// <summary>
        /// The canned ACL to apply to the specified bucket.
        /// </summary>
        public CannedAccessControlList CannedAcl { get; set; }

        /// <summary>
        /// Constructs a new SetBucketAclRequest object, ready to set the specified ACL on the specified bucket when this request is executed.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="acl"></param>
        public SetBucketAclRequest(string bucketName, AccessControlList acl)
        {
            BucketName = bucketName;
            Acl = acl;
        }

        /// <summary>
        /// Constructs a new SetBucketAclRequest object, ready to set the specified canned ACL on the specified bucket when this request is executed.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="cannedAcl"></param>
        public SetBucketAclRequest(string bucketName, CannedAccessControlList cannedAcl)
        {
            BucketName = bucketName;
            CannedAcl = cannedAcl;
        }
 
    }
}
