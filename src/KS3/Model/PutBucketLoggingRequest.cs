using KS3.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        private String bucketName;

        public String BucketName
        {
            get { return bucketName; }
            set { bucketName = value; }
        }
        private void validate() {
            if (String.IsNullOrEmpty(bucketName))
            {
                throw new Exception("bucketname is not null");
            } 
            if(bucketLogging.Enable==true&&bucketLogging.TargetPrefix==null){
                throw new Exception("TargetPrefix is not null");
            }
        }
        private String getXmlContent() {
            validate();
            XNamespace v = "http://s3.amazonaws.com/doc/2006-03-01/";
            XElement root = new XElement(v+"BucketLoggingStatus");
            if(bucketLogging.Enable){
                XElement LoggingEnabled = new XElement("LoggingEnabled");
                LoggingEnabled.Add(new XElement("TargetBucket",bucketLogging.TargetBucket));
                LoggingEnabled.Add(new XElement("TargetPrefix",bucketLogging.TargetPrefix));
                XElement TargetGrants = new XElement("TargetGrants");
                foreach (Grant grant in bucketLogging.TargetGrants)
                {
                    XElement Grant = new XElement("Grant");
                    if(grant.GetType().Equals(typeof(CanonicalGrantee))){
                        XElement grantee = new XElement("Grantee");
                        XNamespace grantNamespace = "http://www.w3.org/2001/XMLSchema-instance";
                        XNamespace grantNamespaceType = "CanonicalUser";
                        grantee.SetAttributeValue(XNamespace.Xmlns + "xsi", grantNamespace);
                        grantee.SetAttributeValue("xsi:type", grantNamespaceType);

                        grantee.Add(new XElement("ID", (grant.getGrantee() as CanonicalGrantee).getIdentifier()));
                        grantee.Add(new XElement("DisplayName", (grant.getGrantee() as CanonicalGrantee).getDisplayName()));
                        Grant.Add(grantee);
                    }
                    if(grant.GetType().Equals(typeof(GroupGrantee))){
                        XElement grantee = new XElement("Grantee");
                        XNamespace grantNamespace = "http://www.w3.org/2001/XMLSchema-instance";
                        XNamespace grantNamespaceType = "Group";
                        grantee.SetAttributeValue(XNamespace.Xmlns + "xsi", grantNamespace);
                        grantee.SetAttributeValue("xsi:type", grantNamespaceType);

                        grantee.Add(new XElement("URI", (grant.getGrantee() as GroupGrantee).getIdentifier()));
                        Grant.Add(grantee);
                    }
                    Grant.Add(new XElement("Permission", grant.getPermission()));
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
            return new MemoryStream(System.Text.Encoding.Default.GetBytes(getXmlContent()));
        }
        //public string getMd5()
        //{
        //    byte[] md5 = Md5Util.md5Digest(getXmlContent());
        //    return Convert.ToBase64String(md5);
        //}
    }
}
