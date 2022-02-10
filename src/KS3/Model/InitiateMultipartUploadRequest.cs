using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class InitiateMultipartUploadRequest:KS3Request
    {
        public InitiateMultipartUploadRequest(String bucketname, String objectkey)
        {
            this.Bucketname=bucketname;
            this.Objectkey=objectkey;
        }

        private String bucketname;

        public String Bucketname
        {
            get { return bucketname; }
            set { bucketname = value; }
        }
        private String objectkey;

        public String Objectkey
        {
            get { return objectkey; }
            set { objectkey = value; }
        }


        private ObjectMetadata objectMeta = new ObjectMetadata();

        public ObjectMetadata ObjectMeta
        {
            get { return objectMeta; }
            set { objectMeta = value; }
        }
 
        private AccessControlList acl = new AccessControlList();

        public AccessControlList Acl
        {
            get { return acl; }
            set { acl = value; }
        }

        private CannedAccessControlList cannedAcl;

        public CannedAccessControlList CannedAcl
        {
            get { return cannedAcl; }
            set { cannedAcl = value; }
        }
        private String redirectLocation;

        public String RedirectLocation
        {
            get { return redirectLocation; }
            set { redirectLocation = value; }
        }


    }
}
