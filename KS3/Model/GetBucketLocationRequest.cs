using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class GetBucketLocationRequest:KS3Request
    {
        private String bucketName;

        public String BucketName
        {
            get { return bucketName; }
            set { bucketName = value; }
        }
        public GetBucketLocationRequest() { }
        public GetBucketLocationRequest(String bucketName) {
            this.bucketName = bucketName;
        }
    }
}
