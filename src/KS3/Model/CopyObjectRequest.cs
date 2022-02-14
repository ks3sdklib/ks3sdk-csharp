namespace KS3.Model
{
    public class CopyObjectRequest : KS3Request
    {
        public string SourceBucket { get; set; }
        public string SourceObject { get; set; }
        public string DestinationBucket { get; set; }
        public string DestinationObject { get; set; }
        public CannedAccessControlList CannedAcl { get; set; }
        public AccessControlList AccessControlList { get; set; }

    }
}
