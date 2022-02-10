using System.IO;

namespace KS3.Model
{
    /// <summary>
    /// Represents an object stored in KS3. This object contains the data content and the object metadata stored by KS3, such as content type, content length, etc.
    /// </summary>
    public class KS3Object
    {
        /// <summary>
        /// The key under which this object is stored
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The name of the bucket in which this object is contained
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// The metadata stored by KS3 for this object
        /// </summary>
        public ObjectMetadata Metadata { get; set; } = new ObjectMetadata();

        /// <summary>
        /// The stream containing the contents of this object from KS3
        /// </summary>
        public Stream ObjectContent { get; set; }


        ~KS3Object()
        {
            ObjectContent?.Close();
            ObjectContent?.Dispose();
        }

        public override string ToString()
        {
            return $"S3Object [key= {Key},bucket= {(string.IsNullOrWhiteSpace(BucketName) ? "<Unknown>" : BucketName)}]";
        }
    }
}
