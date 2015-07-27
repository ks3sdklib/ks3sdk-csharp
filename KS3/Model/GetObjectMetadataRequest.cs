using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    /**
     * <p>
     * Provides options for obtaining the metadata for the specified KS3 object
     * without actually fetching the object contents. This is useful if obtaining
     * only object metadata, and avoids wasting bandwidth from retrieving the object
     * data.
     * </p>
     * <p>
     * The object metadata contains information such as content type, content
     * disposition, etc., as well as custom user metadata that can be associated
     * with an object in KS3.
     * </p>
     */
    public class GetObjectMetadataRequest : KS3Request
    {
        /**
         * The name of the bucket containing the object's whose metadata is being
         * retrieved.
         */
        private String bucketName;

        /**
         * The key of the object whose metadata is being retrieved.
         */
        private String key;

	    /**
	     * Constructs a new GetObjectMetadataRequest used to retrieve a
	     * specified object's metadata.
	     */
        public GetObjectMetadataRequest(String bucketName, String key)
        {
            this.bucketName = bucketName;
            this.key = key;
        }

	    /**
	     * Gets the name of the bucket containing the object whose metadata is being
	     * retrieved.
	     */
        public String getBucketName()
        {
            return this.bucketName;
        }

	    /**
	     * Sets the name of the bucket containing the object whose metadata is being
	     * retrieved.
	     */
        public void setBucketname(String bucketName)
        {
            this.bucketName = bucketName;
        }

	    /**
	     * Gets the key of the object whose metadata is being retrieved.
	     */
        public String getKey()
        {
            return this.key;
        }

	    /**
	     * Sets the key of the object whose metadata is being retrieved.
	     */
        public void setKey(String key)
        {
            this.key = key;
        }
    }
}
