using KS3.Extensions;
using System;
using System.IO;
using System.Xml.Linq;

namespace KS3.Model
{
    public class PutBucketLoggingRequest : KS3Request
    {
        public GetBucketLoggingResult BucketLogging { get; set; }
        public string BucketName { get; set; }

        private void Validate()
        {
            if (BucketName.IsNullOrWhiteSpace())
            {
                throw new Exception("bucketname is not null");
            }

            if (BucketLogging != null)
            {
                if (BucketLogging.Enable == true && BucketLogging.TargetPrefix.IsNullOrWhiteSpace())
                {
                    throw new Exception("TargetPrefix is not null");
                }
            }
        }

        private string GetXmlContent()
        {
            Validate();

            XNamespace v = "http://s3.amazonaws.com/doc/2006-03-01/";
            XElement root = new XElement(v + "BucketLoggingStatus");
            if (BucketLogging.Enable)
            {
                XElement LoggingEnabled = new XElement("LoggingEnabled");
                LoggingEnabled.Add(new XElement("TargetBucket", BucketLogging.TargetBucket));
                LoggingEnabled.Add(new XElement("TargetPrefix", BucketLogging.TargetPrefix));
                XElement TargetGrants = new XElement("TargetGrants");
                foreach (Grant grant in BucketLogging.TargetGrants)
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
        public Stream ToXmlAdapter()
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
