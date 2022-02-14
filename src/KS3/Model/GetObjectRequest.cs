using System;
using System.Collections.Generic;
using System.IO;

namespace KS3.Model
{
    /// <summary>
    /// Provides options for downloading an KS3 object.
    /// </summary>
    public class GetObjectRequest : KS3Request
    {
        /// <summary>
        /// The name of the bucket containing the object to retrieve
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// The key under which the desired object is stored
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Where the content will be stored
        /// </summary>
        public FileInfo DestinationFile { get; set; }

        /// <summary>
        /// Optional member indicating the byte range of data to retrieve
        /// </summary>
        public long[] Range { get; set; }

        /// <summary>
        ///  Optional list of ETag values that constrain this request to only be executed if the object's ETag matches one of the specified ETag values.
        /// </summary>
        public IList<string> MatchingETagConstraints { get; set; } = new List<string>();

        /// <summary>
        ///          * Optional list of ETag values that constrain this request to only be executed if the object's ETag does not match any of the specified ETag constraint values.
        /// </summary>
        public IList<string> NonmatchingETagContraints { get; set; } = new List<string>();

        /// <summary>
        /// Optional field that constrains this request to only be executed if the object has not been modified since the specified date.
        /// </summary>
        public DateTime? UnmodifiedSinceConstraint { get; set; }

        /// <summary>
        /// Optional field that constrains this request to only be executed if the object has been modified since the specified date.
        /// </summary>
        public DateTime? ModifiedSinceConstraint { get; set; }

        /// <summary>
        /// The optional progress listener for receiving updates about object download status.
        /// </summary>
        public IProgressListener ProgressListener { get; set; }

        /// <summary>
        /// Constructs a new GetObjectRequest with all the required parameters.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        public GetObjectRequest(string bucketName, string key)
        {
            BucketName = bucketName;
            Key = key;
        }

        /// <summary>
        /// Constructs a new GetObjectRequest with all the required parameters.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="destinationFile"></param>
        public GetObjectRequest(string bucketName, string key, FileInfo destinationFile)
        {
            BucketName = bucketName;
            Key = key;
            DestinationFile = destinationFile;
        }
    }
}
