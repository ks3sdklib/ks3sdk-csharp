namespace KS3.Model
{
    public class InitiateMultipartUploadResult
    {
        public string Bucket { get; set; }

        public string Key { get; set; }

        public string UploadId { get; set; }

        public override string ToString()
        {
            return $"[bucket:{Bucket}][key:{Key}][uploadid:{UploadId}]";
        }
    }
}
