using KS3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace KS3.Transform
{
    public class MultipartUploadResultUnmarshaller : Unmarshaller<InitiateMultipartUploadResult, Stream>
    {
        public InitiateMultipartUploadResult unmarshall(Stream input)
        {
            InitiateMultipartUploadResult re = new InitiateMultipartUploadResult();
            XDocument doc = XDocument.Load(input);
            var xml = doc.Elements().First().Elements();
            re.setBucket(xml.Where(w => w.Name.LocalName == "Bucket").ToList()[0].Value);
            re.setKey(xml.Where(w => w.Name.LocalName == "Key").ToList()[0].Value);
            re.setUploadId(xml.Where(w => w.Name.LocalName == "UploadId").ToList()[0].Value);
            return re;
        }
    }
}
