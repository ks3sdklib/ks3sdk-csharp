using System;
using System.IO;
using System.Xml.Linq;

namespace KS3.Model
{
    public class PutBucketLoggingRequest : KS3Request
    {
        private GetBucketLoggingResult bucketLogging;

        public GetBucketLoggingResult BucketLogging
        {
            get { return bucketLogging; }
            set { bucketLogging = value; }
        }
        private string bucketName;

        public string BucketName
        {
            get { return bucketName; }
            set { bucketName = value; }
        }
        private void Validate()
        {
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new Exception("bucketname is not null");
            }
            if (bucketLogging.Enable == true && bucketLogging.TargetPrefix == null)
            {
                throw new Exception("TargetPrefix is not null");
            }
        }
        private String GetXmlContent()
        {
            Validate();
            XNamespace v = "http://s3.amazonaws.com/doc/2006-03-01/";
            XElement root = new XElement(v + "BucketLoggingStatus");
            if (bucketLogging.Enable)
            {
                XElement LoggingEnabled = new XElement("LoggingEnabled");
                LoggingEnabled.Add(new XElement("TargetBucket", bucketLogging.TargetBucket));
                LoggingEnabled.Add(new XElement("TargetPrefix", bucketLogging.TargetPrefix));
                XElement TargetGrants = new XElement("TargetGrants");
                foreach (Grant grant in bucketLogging.TargetGrants)
                {
                    XElement Grant = new XElement("Grant");
                    if (grant.GetType().Equals(typeof(CanonicalGrantee)))
                    {
                        XElement grantee = new XElement("Grantee");
                        XNamespace grantNamespace = "http://www.w3.org/2001/XMLSchema-instance";
                        XNamespace grantNamespaceType = "CanonicalUser";
                        grantee.SetAttributeValue(XNamespace.Xmlns + "xsi", grantNamespace);
                        grantee.SetAttributeValue("xsi:type", grantNamespaceType);

                        grantee.Add(new XElement("ID", (grant.Grantee as CanonicalGrantee).GetIdentifier()));
                        grantee.Add(new XElement("DisplayName", (grant.Grantee as CanonicalGrantee).GetDisplayName()));
                        Grant.Add(grantee);
                    }
                    if (grant.GetType().Equals(typeof(GroupGrantee)))
                    {
                        XElement grantee = new XElement("Grantee");
                        XNamespace grantNamespace = "http://www.w3.org/2001/XMLSchema-instance";
                        XNamespace grantNamespaceType = "Group";
                        grantee.SetAttributeValue(XNamespace.Xmlns + "xsi", grantNamespace);
                        grantee.SetAttributeValue("xsi:type", grantNamespaceType);

                        grantee.Add(new XElement("URI", (grant.Grantee as GroupGrantee).GetIdentifier()));
                        Grant.Add(grantee);
                    }
                    Grant.Add(new XElement("Permission", grant.Permission));
                    TargetGrants.Add(Grant);
                }
                LoggingEnabled.Add(TargetGrants);
                root.Add(LoggingEnabled);
            }
            return root.ToString();
        }
        /// <summary>
        /// return the xml stream content
        /// </summary>
        /// <returns></returns>
        public Stream toXmlAdapter()
        {
            return new MemoryStream(System.Text.Encoding.Default.GetBytes(GetXmlContent()));
        }
        //public string getMd5()
        //{
        //    byte[] md5 = Md5Util.md5Digest(getXmlContent());
        //    return Convert.ToBase64String(md5);
        //}
    }
}
