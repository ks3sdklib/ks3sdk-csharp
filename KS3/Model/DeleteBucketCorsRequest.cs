using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class DeleteBucketCorsRequest:KS3Request
    {
        private String bucketName;

        public String BucketName
        {
            get { return bucketName; }
            set { bucketName = value; }
        }
        public DeleteBucketCorsRequest() { }
        public DeleteBucketCorsRequest(String bucketName) {
            this.bucketName = bucketName;
        }

    }
}
