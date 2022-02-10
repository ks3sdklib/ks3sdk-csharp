using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    /**
     * Provides options for deleting a specified bucket. KS3 buckets can only be deleted
     * when empty.
     */
    public class DeleteBucketRequest : KS3Request
    {
        /**
         * The name of the KS3 bucket to delete.
         */
        private String bucketName;

        /**
         * Constructs a new DeleteBucketRequest, 
         * ready to be executed to delete the
         * specified bucket.
         */
        public DeleteBucketRequest(String bucketName)
        {
            this.bucketName = bucketName;
        }

        /**
         * Sets the name of the KS3 bucket to delete.
         */
        public String setBucketName(String bucketName)
        {
            return this.bucketName = bucketName;
        }

        /**
         * Gets the name of the KS3 bucket to delete.
         */
        public String getBucketName()
        {
            return this.bucketName;
        }
    }
}
