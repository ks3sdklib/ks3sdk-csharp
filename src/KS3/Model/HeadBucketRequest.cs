using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class HeadBucketRequest:KS3Request
    {
        /** The name of the bucket whose ACL is being retrieved. */
        private String bucketName;

        public String BucketName
        {
            get { return bucketName; }
            set { bucketName = value; }
        }
        public HeadBucketRequest() { }
        public HeadBucketRequest(String bucketName)
        {
            this.bucketName = bucketName;
        }
    }
}
