namespace KS3.Model
{
    public class AbortMultipartUploadRequest : KS3Request
    {
        public string BucketName { get; set; }
        public string ObjectKey { get; set; }
        public string UploadId { get; set; }


        public AbortMultipartUploadRequest() { }

        public AbortMultipartUploadRequest(string bucketName, string objectKey, string uploadId)
        {
            BucketName = bucketName;
            ObjectKey = objectKey;
            UploadId = uploadId;
        }
    }
}
