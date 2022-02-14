namespace KS3.Model
{
    public class PartETag
    {
        public int PartNumber { get; set; }
        public string ETag { get; set; }
        public PartETag()
        {
        }

        public PartETag(int partNumber, string eTag)
        {
            PartNumber = partNumber;
            ETag = eTag;
        }

        public override string ToString()
        {
            return $"[partNum:{PartNumber}][etag:{ETag}]";
        }

    }
}
