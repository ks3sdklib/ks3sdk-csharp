using KS3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace KS3.Transform
{
    public class CompleteMultipartUploadResultUnmarshaller : Unmarshaller<CompleteMultipartUploadResult, Stream>
    {

        public CompleteMultipartUploadResult unmarshall(Stream input)
        {
            CompleteMultipartUploadResult re = new CompleteMultipartUploadResult();
            XDocument doc = XDocument.Load(input);
            var xml = doc.Elements().First().Elements();
            re.setBucket(xml.Where(w => w.Name.LocalName == "Bucket").ToList()[0].Value);
            re.setKey(xml.Where(w => w.Name.LocalName == "Key").ToList()[0].Value);
            re.seteTag(xml.Where(w => w.Name.LocalName == "ETag").ToList()[0].Value);
            return re;
        }
    }
}
