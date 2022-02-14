namespace KS3.Model
{
    public class SetObjectAclRequest : KS3Request
    {
        /// <summary>
        /// The name of the bucket whose object's ACL is being set.
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// The key of the object whose ACL is being set.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The custom ACL to apply to the specified object.
        /// </summary>
        public AccessControlList Acl { get; set; }

        /// <summary>
        /// The canned ACL to apply to the specified object.
        /// </summary>
        public CannedAccessControlList CannedAcl { get; set; }


        /// <summary>
        /// Constructs a new SetObjectAclRequest object, ready to set the specified ACL on the specified object when this request is executed.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="acl"></param>
        public SetObjectAclRequest(string bucketName, string key, AccessControlList acl)
        {
            BucketName = bucketName;
            Key = key;
            Acl = acl;
        }

        /// <summary>
        /// Constructs a new SetObjectAclRequest object, ready to set the specified ACL on the specified object when this request is executed.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="cannedAcl"></param>
        public SetObjectAclRequest(string bucketName, string key, CannedAccessControlList cannedAcl)
        {
            BucketName = bucketName;
            Key = key;
            CannedAcl = cannedAcl;
        }
    }
}
