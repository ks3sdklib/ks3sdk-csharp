using KS3.Model;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace KS3.Transform
{
    public class CopyObjectResultUnmarshaller : IUnmarshaller<CopyObjectResult, Stream>
    {
        public CopyObjectResult Unmarshall(Stream input)
        {
            CopyObjectResult result=new CopyObjectResult();
            XDocument doc = XDocument.Load(input);
            var xml = doc.Elements();
            result.LastModified = Convert.ToDateTime(xml.First().Elements().ElementAt(0).Value);
            result.ETag = xml.First().Elements().ElementAt(1).Value;
            return result;
        }
    }
}
