using KS3.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace KS3.Model
{
    public class DeleteMultipleObjectsRequest:KS3Request,ICalculatorMd5
    {
        private String bucketName;

        public String BucketName
        {
            get { return bucketName; }
            set { bucketName = value; }
        }
        private String[] objectKeys;

        public String[] ObjectKeys
        {
            get { return objectKeys; }
            set { objectKeys = value; }
        }
        private String getXmlContent()
        {
            validate();
            //XNamespace v = "http://s3.amazonaws.com/doc/2006-03-01/";
            XElement root = new XElement("Delete");
            foreach(String key in objectKeys){
                XElement Object = new XElement("Object");
                Object.Add(new XElement("Key",key));
                root.Add(Object);
            }
            return root.ToString();
        }
        public Stream toXmlAdapter()
        {
            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(getXmlContent()));
        }
        /// <summary>
        /// get the md5 digest byte and convert to base64 string
        /// </summary>
        /// <returns></returns>
        public String GetMd5()
        {
            byte[] md5 = Md5Util.Md5Digest(getXmlContent());
            return Convert.ToBase64String(md5);
        }

        private void validate()
        {
            if (String.IsNullOrEmpty(bucketName))
            {
                throw new Exception("bucketname is not null");
            }
            if (objectKeys == null || objectKeys.Length == 0)
            {
                throw new Exception("objectKeys is not null");
            }
        }

    }
}
