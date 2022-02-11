namespace KS3.Model
{
    /// <summary>
    /// Request object containing all the options for requesting an object's Access Control List (ACL).
    /// </summary>
    public class GetObjectAclRequest : KS3Request
    {
        /// <summary>
        /// The name of the bucket whose object's ACL is being retrieved.
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// The key of the object whose ACL is being retrieved. 
        /// </summary>
        public string Key { get; set; }

        public GetObjectAclRequest()
        {

        }

        /// <summary>
        /// Constructs a new GetObjectAclRequest object, ready to retrieve the ACL for the specified bucket when executed.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        public GetObjectAclRequest(string bucketName, string key)
        {
            BucketName = bucketName;
            Key = key;
        }


    }
}
