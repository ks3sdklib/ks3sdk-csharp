using KS3.Http;
using KS3.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace KS3.Model
{
    public class PutBucketCorsRequest : KS3Request, CalculatorMd5
    {
        private String bucketName;

        public String BucketName
        {
            get { return bucketName; }
            set { bucketName = value; }
        }

        private BucketCorsConfigurationResult bucketCorsConfiguration;

        public BucketCorsConfigurationResult BucketCorsConfiguration
        {
            get { return bucketCorsConfiguration; }
            set { bucketCorsConfiguration = value; }
        }
        public PutBucketCorsRequest() { }
        public PutBucketCorsRequest(String bucketName, BucketCorsConfigurationResult bucketCorsConfiguration)
        {
            this.bucketName = bucketName;
            this.bucketCorsConfiguration = bucketCorsConfiguration;
        }
        private String getXmlContent() {
            validate();
            XNamespace v = "http://s3.amazonaws.com/doc/2006-03-01/";
            XElement root = new XElement(v+"CORSConfiguration");
            
            foreach (CorsRule cr in bucketCorsConfiguration.Rules)
            {
                XElement CORSRule = new XElement("CORSRule");
                foreach (String origin in cr.AllowedOrigins)
                {
                    CORSRule.Add(new XElement("AllowedOrigin", origin));
                }
                foreach (String header in cr.AllowedHeaders)
                {
                    CORSRule.Add(new XElement("AllowedHeader", header));
                }
                foreach (String eheader in cr.ExposedHeaders)
                {
                    CORSRule.Add(new XElement("ExposeHeader", eheader));
                }
                foreach (HttpMethod method in cr.AllowedMethods)
                {
                    CORSRule.Add(new XElement("AllowedMethod", method.ToString()));
                }
                CORSRule.Add(new XElement("MaxAgeSeconds", cr.MaxAgeSeconds));
                root.Add(CORSRule);
            }
            return root.ToString();
        }
        /// <summary>
        /// return the xml stream content
        /// </summary>
        /// <returns></returns>
        public Stream toXmlAdapter(){
            return new MemoryStream(System.Text.Encoding.Default.GetBytes(getXmlContent()));
        }
        /// <summary>
        /// get the md5 digest byte and convert to base64 string
        /// </summary>
        /// <returns></returns>
        public String getMd5()
        {
            byte[] md5 = Md5Util.md5Digest(getXmlContent());
            return Convert.ToBase64String(md5);
        }
        
        private void validate() {
            if (String.IsNullOrEmpty(bucketName))
            {
                throw new Exception("bucketname is not null");
            }
            if (bucketCorsConfiguration.Rules == null || bucketCorsConfiguration.Rules.Count==0)
            {
                throw new Exception("cors rules is not null");
            }
            if (bucketCorsConfiguration.Rules.Count > Constants.corsMaxRules)
            {
                throw new Exception("cors rules number must limit in " + Constants.corsMaxRules);
            }
            foreach (CorsRule cr in bucketCorsConfiguration.Rules)
            {
                if (cr.AllowedMethods == null || cr.AllowedMethods.Count == 0) {
                    throw new Exception("bucketCorsConfiguration.rules.allowedMethods not null");
                }
                if(cr.AllowedOrigins==null||cr.AllowedOrigins.Count==0){
                    throw new Exception("bucketCorsConfiguration.rules.allowedOrigins not null");
                }
            }
        }
    }
}
