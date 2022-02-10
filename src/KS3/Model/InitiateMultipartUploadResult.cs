using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class InitiateMultipartUploadResult
    {

        private String bucket;

        private String key;

        private String uploadId;
        public String getBucket()
        {
            return bucket;
        }
        public void setBucket(String bucket)
        {
            this.bucket = bucket;
        }
        public String getKey()
        {
            return key;
        }
        public void setKey(String key)
        {
            this.key = key;
        }
        public String getUploadId()
        {
            return uploadId;
        }
        public void setUploadId(String uploadId)
        {
            this.uploadId = uploadId;
        }
        public String ToString() {
            return "[bucket:"+bucket+"][key:"+key+"][uploadid:"+uploadId+"]";
        }
    }
}
