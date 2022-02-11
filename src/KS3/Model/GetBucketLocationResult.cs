using KS3.Http;

namespace KS3.Model
{
    public class GetBucketLocationResult
    {
        public Region Region { get; set; }

        public GetBucketLocationResult()
        {

        }

        public GetBucketLocationResult(Region region)
        {
            Region = region;
        }
    }
}
