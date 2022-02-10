using KS3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace KS3.Transform
{
    public class GetAdpResultUnmarshaller : IUnmarshaller<GetAdpResult, Stream>
    {
        public GetAdpResult Unmarshall(Stream input)
        {
            GetAdpResult result = new GetAdpResult();
            XDocument doc = XDocument.Load(input);
            result.Doc = doc;
            return result;
        }
    }
}
