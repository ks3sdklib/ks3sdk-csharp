using KS3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace KS3.Transform
{
    public class DeleteMultipleObjectsResultUnmarshaller : IUnmarshaller<DeleteMultipleObjectsResult, Stream>
    {
        public DeleteMultipleObjectsResult Unmarshall(Stream input)
        {
            DeleteMultipleObjectsResult reuslt = new DeleteMultipleObjectsResult();
            XDocument doc = XDocument.Load(input);
            var xml = doc.Elements();
            return reuslt;
        }
    }
}
