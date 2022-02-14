using System.IO;

namespace KS3.Model
{
    public class CompleteMultipartUploadRequest : KS3Request
    {
        public string BucketName { get; set; }
        public string ObjectKey { get; set; }
        public string UploadId { get; set; }
        public Stream Content { get; set; }

        public CompleteMultipartUploadRequest() { }

        public CompleteMultipartUploadRequest(string bucketName, string objectKey, string uploadId) : this(bucketName, objectKey, uploadId, null)
        {
            
        }

        public CompleteMultipartUploadRequest(string bucketName, string objectKey, string uploadId, Stream content)
        {
            BucketName = bucketName;
            ObjectKey = objectKey;
            UploadId = uploadId;
            Content = content;
        }


    }
}
