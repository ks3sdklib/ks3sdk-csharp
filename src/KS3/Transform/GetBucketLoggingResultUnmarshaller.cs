using KS3.Model;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace KS3.Transform
{
    public class GetBucketLoggingResultUnmarshaller : IUnmarshaller<GetBucketLoggingResult, Stream>
    {
        public GetBucketLoggingResult Unmarshall(Stream input)
        {
            GetBucketLoggingResult result = new GetBucketLoggingResult();
            XDocument doc = XDocument.Load(input);
            var xml = doc.Elements();
            var bucketLoggingStatus = xml.First().Elements();
            if (bucketLoggingStatus != null && bucketLoggingStatus.Count()>0)
            {
                result.Enable = true;
                result.TargetBucket = bucketLoggingStatus.First().Elements().ElementAt(0).Value;
                result.TargetPrefix = bucketLoggingStatus.First().Elements().ElementAt(1).Value;
            }
            return result;
        }
    }
}
