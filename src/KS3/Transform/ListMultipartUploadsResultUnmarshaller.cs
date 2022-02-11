using KS3.Extensions;
using KS3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace KS3.Transform
{
    public class ListMultipartUploadsResultUnmarshaller : IUnmarshaller<ListMultipartUploadsResult, Stream>
    {
        public ListMultipartUploadsResult Unmarshall(Stream input)
        {
            ListMultipartUploadsResult re = new ListMultipartUploadsResult();
            XDocument doc = XDocument.Load(input);
            var xml = doc.Elements().First().Elements();


            re.BucketName = xml.GetSingleValueOrDefault("Bucket");
            re.ObjectKey = xml.GetSingleValueOrDefault("Key");
            re.UploadId = xml.GetSingleValueOrDefault("UploadId");
            re.IsTruncated = Convert.ToBoolean(xml.GetSingleValueOrDefault("IsTruncated", "false"));

            var plist = new List<Part>();
            var parts = xml.Where(x => x.Name.LocalName == "Part").ToList();
            foreach (var item in parts)
            {
                Part p = new Part
                {
                    PartNumber = Convert.ToInt32(item.Element("PartNumber").Value),
                    ETag = item.Element("ETag").Value,
                    LastModified = Convert.ToDateTime(item.Element("LastModified").Value),
                    Size = Convert.ToInt32(item.Element("Size").Value)
                };
                plist.Add(p);
            }

            re.Parts = plist;
            return re;
        }
    }
}
