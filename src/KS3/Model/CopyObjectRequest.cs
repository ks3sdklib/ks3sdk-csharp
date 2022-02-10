using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class CopyObjectRequest:KS3Request
    {
        private String sourceBucket;

        public String SourceBucket
        {
            get { return sourceBucket; }
            set { sourceBucket = value; }
        }
        private String sourceObject;

        public String SourceObject
        {
            get { return sourceObject; }
            set { sourceObject = value; }
        }
        private String destinationBucket;

        public String DestinationBucket
        {
            get { return destinationBucket; }
            set { destinationBucket = value; }
        }
        private String destinationObject;

        public String DestinationObject
        {
            get { return destinationObject; }
            set { destinationObject = value; }
        }
        private CannedAccessControlList cannedAcl;

        public CannedAccessControlList CannedAcl
        {
            get { return cannedAcl; }
            set { cannedAcl = value; }
        }
        private AccessControlList accessControlList;

        public AccessControlList AccessControlList
        {
            get { return accessControlList; }
            set { accessControlList = value; }
        }
    }
}
