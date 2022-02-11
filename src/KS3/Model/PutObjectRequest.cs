using System.IO;

namespace KS3.Model
{
    /// <summary>
    /// Uploads a new object to the specified KS3 bucket.
    /// </summary>
    public class PutObjectRequest : KS3Request
    {
        /// <summary>
        /// The name of an existing bucket, to which this request will upload a new object.
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// The key under which to store the new object.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The file containing the data to be uploaded to KS3. You must either specify a file or a Stream containing the data to be uploaded to KS3.
        /// </summary>
        public FileInfo File { get; set; }

        /// <summary>
        /// The Stream containing the data to be uploaded to KS3. You must either specify a file or an Stream containing the data to be uploaded to KS3.
        /// </summary>
        public Stream InputStream { get; set; }

        /// <summary>
        /// Optional metadata instructing KS3 how to handle the uploaded data (e.g. custom user metadata, hooks for specifying content type, etc.). If you are uploading from an Stream, you <bold>should always</bold> specify metadata with the content size set, otherwise the contents of the Stream will have to be buffered in memory before they can be sent to KS3, which can have very negative performance impacts.
        /// </summary>
        public ObjectMetadata Metadata { get; set; }

        /// <summary>
        /// An optional pre-configured access control policy to use for the new object. Ignored in favor of accessControlList, if present.
        /// </summary>
        public CannedAccessControlList CannedAcl { get; set; }

        /// <summary>
        /// An optional access control list to apply to the new object. If specified, cannedAcl will be ignored.
        /// </summary>
        public AccessControlList Acl { get; set; }

        /// <summary>
        /// The optional progress listener for receiving updates about object upload status.
        /// </summary>
        public IProgressListener ProgressListener { get; set; }

        /// <summary>
        /// Constructs a new PutObjectRequest object to upload a file to the specified bucket and key. After constructing the request, users may optionally specify object metadata or a canned ACL as well.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="file"></param>
        public PutObjectRequest(string bucketName, string key, FileInfo file)
        {
            BucketName = bucketName;
            Key = key;
            File = file;
        }

        /// <summary>
        /// Constructs a new PutObjectRequest object to upload a stream of data to the specified bucket and key. After constructing the request, users may optionally specify object metadata or a canned ACL as well.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="input"></param>
        /// <param name="metadata"></param>
        public PutObjectRequest(string bucketName, string key, Stream input, ObjectMetadata metadata)
        {
            BucketName = bucketName;
            Key = key;
            InputStream = input;
            Metadata = metadata;
        }
    }
}
