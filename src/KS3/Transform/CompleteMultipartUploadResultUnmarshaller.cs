using KS3.Extensions;
using KS3.Model;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace KS3.Transform
{
    public class CompleteMultipartUploadResultUnmarshaller : IUnmarshaller<CompleteMultipartUploadResult, Stream>
    {

        public CompleteMultipartUploadResult Unmarshall(Stream input)
        {
            CompleteMultipartUploadResult re = new CompleteMultipartUploadResult();
            XDocument doc = XDocument.Load(input);
            var xml = doc.Elements().First().Elements();

            re.Bucket = xml.GetSingleValueOrDefault("Bucket");
            re.Key = xml.GetSingleValueOrDefault("Key");
            re.ETag = xml.GetSingleValueOrDefault("ETag");

            //re.Bucket = xml.Where(w => w.Name.LocalName == "Bucket").ToList()[0].Value;
            //re.Key = xml.Where(w => w.Name.LocalName == "Key").ToList()[0].Value;
            //re.ETag = xml.Where(w => w.Name.LocalName == "ETag").ToList()[0].Value;
            return re;
        }
    }
}
