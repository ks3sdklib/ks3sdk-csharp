namespace KS3.Model
{
    public class ListMultipartUploadsRequest : KS3Request
    {

        public string BucketName { get; set; }
        public string ObjectKey { get; set; }
        public string UploadId { get; set; }

        public ListMultipartUploadsRequest()
        {
        }

        public ListMultipartUploadsRequest(string bucketName, string objectKey, string uploadId)
        {
            BucketName = bucketName;
            ObjectKey = objectKey;
            UploadId = uploadId;
        }
    }
}
