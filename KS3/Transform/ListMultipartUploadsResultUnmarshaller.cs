using KS3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace KS3.Transform
{
    public class ListMultipartUploadsResultUnmarshaller : Unmarshaller<ListMultipartUploadsResult, Stream>
    {
        public ListMultipartUploadsResult unmarshall(Stream input)
        {
            ListMultipartUploadsResult re = new ListMultipartUploadsResult();
            XDocument doc = XDocument.Load(input);
            var xml=doc.Elements().First().Elements();

            re.setBucketname(xml.Where(w => w.Name.LocalName == "Bucket").ToList()[0].Value);
            re.setObjectkey(xml.Where(w => w.Name.LocalName == "Key").ToList()[0].Value);
            re.setUploadId(xml.Where(w => w.Name.LocalName == "UploadId").ToList()[0].Value);
            re.setIsTruncated(Convert.ToBoolean(xml.Where(w => w.Name.LocalName == "IsTruncated").ToList()[0].Value));
            IList<Part> plist = new List<Part>();
            var parts=xml.Where(x=>x.Name.LocalName=="Part").ToList();
            foreach(var item in parts){
                Part p = new Part();
                p.PartNumber=Convert.ToInt32(item.Element("PartNumber").Value);
                p.ETag = item.Element("ETag").Value;
                p.LastModified = Convert.ToDateTime(item.Element("LastModified").Value);
                p.Size = Convert.ToInt32(item.Element("Size").Value);
                plist.Add(p);
            }
            re.setParts(plist);
            return re;
        }
    }
}
