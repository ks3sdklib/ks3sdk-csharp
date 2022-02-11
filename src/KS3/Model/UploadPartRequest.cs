using System.IO;

namespace KS3.Model
{
    public class UploadPartRequest : KS3Request
    {
        public string BucketName { get; set; }
        public string ObjectKey { get; set; }
        public string UploadId { get; set; }
        public int PartNumber { get; set; }
        public ObjectMetadata Metadata { get; set; }
        public Stream InputStream { get; set; }
        public IProgressListener ProgressListener { get; set; }

        public UploadPartRequest()
        {

        }

        public UploadPartRequest(string bucketName, string objectKey, string uploadId, int partNumber)
        {
            BucketName = bucketName;
            ObjectKey = objectKey;
            UploadId = uploadId;
            PartNumber = partNumber;
        }
    }
}
