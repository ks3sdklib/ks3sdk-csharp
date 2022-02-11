namespace KS3.Model
{
    /// <summary>
    ///  * Provides options for obtaining the metadata for the specified KS3 object without actually fetching the object contents.This is useful if obtaining
    ///  * only object metadata, and avoids wasting bandwidth from retrieving the object data.
    ///  * The object metadata contains information such as content type, content disposition, etc., as well as custom user metadata that can be associated with an object in KS3.
    /// </summary>
    public class GetObjectMetadataRequest : KS3Request
    {
        /// <summary>
        ///  * The name of the bucket containing the object's whose metadata is being retrieved.
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// The key of the object whose metadata is being retrieved.
        /// </summary>
        public string Key { get; set; }

        public GetObjectMetadataRequest()
        {

        }

        public GetObjectMetadataRequest(string bucketName, string key)
        {
            BucketName = bucketName;
            Key = key;
        }
    }
}
