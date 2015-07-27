using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class AbortMultipartUploadRequest:KS3Request
    {
        private String bucketname;
        private String objectkey;
        private String uploadId;
 
        public AbortMultipartUploadRequest() { }
        public AbortMultipartUploadRequest(String bucketname, String objectKey, String uploadId)
        {
            this.bucketname = bucketname;
            this.objectkey = objectKey;
            this.uploadId = uploadId;
        }
        public void setBucketname(String bucketname)
        {
            this.bucketname = bucketname;
        }
        public String getBucketname()
        {
            return this.bucketname;
        }
        public void setObjectkey(String objectkey)
        {
            this.objectkey = objectkey;
        }
        public string getObjectkey()
        {
            return this.objectkey;
        }
        public String getUploadId()
        {
            return uploadId;
        }

        public void setUploadId(String uploadId)
        {
            this.uploadId = uploadId;
        }
        
    }
}
