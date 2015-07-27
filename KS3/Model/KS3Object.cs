using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KS3.Model
{
    /**
     * Represents an object stored in KS3. This object contains the data content and
     * the object metadata stored by KS3, such as content type, content length, etc.
     */
    public class KS3Object
    {
        /** The key under which this object is stored */
        private String key = null;

        /** The name of the bucket in which this object is contained */
        private String bucketName = null;

        /** The metadata stored by KS3 for this object */
        private ObjectMetadata metadata = new ObjectMetadata();

        /** The stream containing the contents of this object from KS3 */
        private Stream objectContent;

        ~KS3Object()
        {
            if (this.objectContent != null)
                this.objectContent.Close();
        }

	    /**
	     * Gets the metadata stored by KS3 for this object. 
         */
        public ObjectMetadata getObjectMetadata()
        {
            return metadata;
        }

	    /**
	     * Sets the object metadata for this object.
	     */
        public void setObjectMetadata(ObjectMetadata metadata)
        {
            this.metadata = metadata;
        }

	    /**
	     * Gets an input stream containing the contents of this object.
	     */
        public Stream getObjectContent()
        {
            return objectContent;
        }

	    /**
	     * Sets the input stream containing this object's contents.
	     */
        public void setObjectContent(Stream objectContent)
        {
            this.objectContent = objectContent;
        }

	    /**
	     * Gets the name of the bucket in which this object is contained.
	     */
        public String getBucketName()
        {
            return bucketName;
        }

	    /**
	     * Sets the name of the bucket in which this object is contained.
	     */
        public void setBucketName(String bucketName)
        {
            this.bucketName = bucketName;
        }

	    /**
	     * Gets the key under which this object is stored.
	     */
        public String getKey()
        {
            return key;
        }

	    /**
	     * Sets the key under which this object is stored.
	     */
        public void setKey(String key)
        {
            this.key = key;
        }

        public override String ToString()
        {
            return "S3Object [key=" + getKey()
                + ",bucket=" + (bucketName == null ? "<Unknown>" : bucketName)
                + "]";
        }
    }
}
