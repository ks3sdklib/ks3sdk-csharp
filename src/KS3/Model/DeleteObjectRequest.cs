using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    /**
     * Provides options for deleting a specified object in a specified bucket. 
     */
    public class DeleteObjectRequest : KS3Request
    {
        /**
         * The name of the KS3 bucket containing the object to delete.
         */
        private String bucketName;

        /**
         * The key of the object to delete.
         */
        private String key;

	    /**
	     * Constructs a new DeleteObjectRequest, specifying the object's
	     * bucket name and key.
         */
        public DeleteObjectRequest(String bucketName, String key)
        {
            this.bucketName = bucketName;
            this.key = key;
        }

	    /**
	     * Gets the name of the KS3 bucket containing the object to delete.
	     */
        public String getBucketName()
        {
            return this.bucketName;
        }

	    /**
	     * Sets the name of the KS3 bucket containing the object to delete.
	     */
        public void setBucketName(String bucketName)
        {
            this.bucketName = bucketName;
        }

	    /**
	     * Gets the key of the object to delete.
	     */
        public String getKey()
        {
            return this.key;
        }

	    /**
	     * Sets the key of the object to delete.
	     */
        public void setKey(String key)
        {
            this.key = key;
        }
    }
}
