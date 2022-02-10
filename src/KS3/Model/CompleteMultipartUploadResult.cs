using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class CompleteMultipartUploadResult
    {
        /**
	 * 新建对象的uri
	 */
        private String location;
        /**
         * 新建object存放的bucket
         */
        private String bucket;
        /**
         * 新建object的object key
         */
        private String key;
        /**
         * 新建object的etag
         */
        private String eTag;
        public String getLocation()
        {
            return location;
        }
        public void setLocation(String location)
        {
            this.location = location;
        }
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
        public String geteTag()
        {
            return eTag;
        }
        public void seteTag(String eTag)
        {
            this.eTag = eTag;
        }
    }
}
