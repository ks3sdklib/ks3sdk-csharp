using KS3.Http;
using KS3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace KS3.Transform
{
    class BucketCorsConfigurationResultUnmarshaller : IUnmarshaller<BucketCorsConfigurationResult, Stream>
    {
        public BucketCorsConfigurationResult Unmarshall(Stream input)
        {
            BucketCorsConfigurationResult result = new BucketCorsConfigurationResult();
            CorsRule corsRule = null;
            XDocument doc = XDocument.Load(input);
            var xml = doc.Elements();
            var rules=xml.First().Elements().Where(w => w.Name == "CORSRule").ToList();
            foreach(var rule in rules){
                corsRule = new CorsRule();
                var AllowedOrigins = rule.Elements().Where(w => w.Name == "AllowedOrigin").ToList();
                foreach (var o in AllowedOrigins)
                {
                    corsRule.AllowedOrigins.Add(o.Value);
                }
                var AllowedMethod = rule.Elements().Where(w => w.Name == "AllowedMethod").ToList();
                foreach(var m in AllowedMethod){
                    corsRule.AllowedMethods.Add((HttpMethod)Enum.Parse(typeof(HttpMethod), m.Value));
                }

                var AllowedHeader = rule.Elements().Where(w => w.Name == "AllowedHeader").ToList();
                foreach(var h in AllowedHeader){
                    corsRule.AllowedHeaders.Add(h.Value);
                }
                corsRule.MaxAgeSeconds =Convert.ToInt32(rule.Element("MaxAgeSeconds").Value);
                var ExposeHeader = rule.Elements().Where(w => w.Name == "ExposeHeader").ToList();
                foreach(var eh in ExposeHeader){
                    corsRule.ExposedHeaders.Add(eh.Value);
                }
                result.Rules.Add(corsRule);
            }
            return result;
        }
    }
}
