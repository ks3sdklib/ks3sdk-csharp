namespace KS3.Model
{
    public class InitiateMultipartUploadRequest : KS3Request
    {
        public string BucketName { get; set; }

        public string ObjectKey { get; set; }

        public ObjectMetadata ObjectMeta { get; set; } = new ObjectMetadata();

        public AccessControlList Acl { get; set; } = new AccessControlList();

        public CannedAccessControlList CannedAcl { get; set; }

        public string RedirectLocation { get; set; }

        public InitiateMultipartUploadRequest()
        {

        }

        public InitiateMultipartUploadRequest(string bucketName, string objectKey)
        {
            BucketName = bucketName;
            ObjectKey = objectKey;
        }
    }
}
