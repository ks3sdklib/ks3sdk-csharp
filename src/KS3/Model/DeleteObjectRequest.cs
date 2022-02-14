namespace KS3.Model
{
    /// <summary>
    /// Provides options for deleting a specified object in a specified bucket. 
    /// </summary>
    public class DeleteObjectRequest : KS3Request
    {
        /// <summary>
        /// The name of the KS3 bucket containing the object to delete.
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// The key of the object to delete.
        /// </summary>
        public string Key { get; set; }

        public DeleteObjectRequest()
        {

        }

        public DeleteObjectRequest(string bucketName, string key)
        {
            BucketName = bucketName;
            Key = key;
        }
    }
}
