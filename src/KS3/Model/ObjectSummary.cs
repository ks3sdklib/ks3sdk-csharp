using KS3.Extensions;
using System;
using System.Text;

namespace KS3.Model
{
    /// <summary>
    /// Contains the summary of an object stored in an KS3 bucket. This object doesn't contain contain the object's full metadata or any of its contents.
    /// </summary>
    public class ObjectSummary
    {
        /// <summary>
        /// The name of the bucket in which this object is stored
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// The name of the bucket in which this object is stored
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Hex encoded MD5 hash of this object's contents, as computed by KS3 
        /// </summary>
        public string ETag { get; set; }

        /// <summary>
        /// The size of this object, in bytes 
        /// </summary>
        public long? Size { get; set; }

        /// <summary>
        /// The date, according to KS3, when this object was last modified
        /// </summary>
        public DateTime? LastModified { get; set; }

        /// <summary>
        ///  The owner of this object - can be null if the requester doesn't have permission to view object ownership information
        /// </summary>
        public Owner Owner { get; set; }


        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"ObjectSummary [bucketName={BucketName}, owner={Owner}");
            if (!Key.IsNullOrWhiteSpace())
            {
                builder.Append($", key={Key}");
            }
            if (!ETag.IsNullOrWhiteSpace())
            {
                builder.Append($", eTag={ETag}");
            }
            if (Size.HasValue)
            {
                builder.Append($", size={Size}");
            }
            if (LastModified.HasValue)
            {
                builder.Append($", lastModified={LastModified}");
            }

            builder.Append("]");
            return builder.ToString();
        }
    }
}
