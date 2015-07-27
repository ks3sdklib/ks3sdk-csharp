using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class ListMultipartUploadsRequest:KS3Request
    {
        private String bucketname;
        private String objectkey;
        private String uploadId;
        public ListMultipartUploadsRequest() { }
        public ListMultipartUploadsRequest(string bucket, string objKey, string uploadId)
        {
            this.bucketname = bucket;
            this.objectkey = objKey;
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
