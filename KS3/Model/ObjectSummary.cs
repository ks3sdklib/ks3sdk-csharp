using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    /**
     * Contains the summary of an object stored in an KS3 bucket. This object
     * doesn't contain contain the object's full metadata or any of its contents.
     */
    public class ObjectSummary
    {
        /** The name of the bucket in which this object is stored */
        private String bucketName;

        /** The key under which this object is stored */
        private String key;

        /** Hex encoded MD5 hash of this object's contents, as computed by KS3 */
        private String eTag;

        /** The size of this object, in bytes */
        private long? size;

        /** The date, according to KS3, when this object was last modified */
        private DateTime lastModified;

        /**
         * The owner of this object - can be null if the requester doesn't have
         * permission to view object ownership information
         */
        private Owner owner;

	    /**
	     * Gets the name of the KS3 bucket in which this object is stored.
	     */
        public String getBucketName()
        {
            return this.bucketName;
        }

	    /**
	     * Sets the name of the KS3 bucket in which this object is stored.
	     */
        public void setBucketName(String bucketName)
        {
            this.bucketName = bucketName;
        }

        /**
         * Gets the key under which this object is stored in KS3.
         */
        public String getKey()
        {
            return key;
        }

        /**
         * Sets the key under which this object is stored in KS3.
         */
        public void setKey(String key)
        {
            this.key = key;
        }

	    /**
	     * Gets the hex encoded 128-bit MD5 hash of this object's contents as
	     * computed by KS3.
	     */
        public String getETag()
        {
            return this.eTag;
        }

	    /**
	     * Sets the hex encoded 128-bit MD5 hash of this object's contents as
	     * computed by KS3.
	     */
        public void setETag(String eTag)
        {
            this.eTag = eTag;
        }

	    /**
	     * Gets the size of this object in bytes.
	     */
        public long? getSize()
        {
            return this.size;
        }

	    /**
	     * Sets the size of this object in bytes.
	     */
        public void setSize(long? size)
        {
            this.size = size;
        }

	    /**
	     * Gets the date when, according to KS3, this object was last
	     * modified.
	     */
        public DateTime getLastModified()
        {
            return this.lastModified;
        }

	    /**
	     * Sets the date, according to KS3, this object was last modified.
	     */
        public void setLastModified(DateTime lastModified)
        {
            this.lastModified = lastModified;
        }

	    /**
	     * Gets the owner of this object.
         */
        public Owner getOwner()
        {
            return this.owner;
        }

	    /**
	     * Sets the owner of this object.
	     */
        public void setOwner(Owner owner)
        {
            this.owner = owner;
        }


        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("ObjectSummary [bucketName=" + this.bucketName + ", owner=" + this.owner);
            if (this.key != null) builder.Append(", key=" + this.key);
            if (this.eTag != null) builder.Append(", eTag=" + this.eTag);
            if (this.size != null) builder.Append(", size=" + this.size);
            if (this.lastModified != null) builder.Append(", lastModified=" + this.lastModified);
            builder.Append("]");
            return builder.ToString();
        }
    }
}
