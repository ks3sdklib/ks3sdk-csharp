using KS3.Model;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace KS3.Transform
{
    public class MultipartUploadResultUnmarshaller : IUnmarshaller<InitiateMultipartUploadResult, Stream>
    {
        public InitiateMultipartUploadResult Unmarshall(Stream input)
        {
            InitiateMultipartUploadResult re = new InitiateMultipartUploadResult();
            XDocument doc = XDocument.Load(input);
            var xml = doc.Elements().First().Elements();
            re.Bucket = xml.Where(w => w.Name.LocalName == "Bucket").ToList()[0].Value;
            re.Key = xml.Where(w => w.Name.LocalName == "Key").ToList()[0].Value;
            re.UploadId = xml.Where(w => w.Name.LocalName == "UploadId").ToList()[0].Value;
            return re;
        }
    }
}
