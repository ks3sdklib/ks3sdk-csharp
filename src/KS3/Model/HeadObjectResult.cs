namespace KS3.Model
{
    public class HeadObjectResult
    {
        public ObjectMetadata ObjectMetadata { get; set; } = new ObjectMetadata();
        public bool IfModified { get; set; }
        public bool IfPreconditionSuccess { get; set; }
    }
}
