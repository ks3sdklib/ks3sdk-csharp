using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class Bucket
    {
        /** The name of this KS3 bucket */
        private String name = null;

        /** The details on the owner of this bucket */
        private Owner owner = null;

        /** The date this bucket was created */
        private DateTime? creationDate = null;

        public Bucket() { }

        public Bucket(String name)
        {
            this.name = name;
        }

        public override String ToString()
        {
            return "kS3Bucket [name=" + this.getName() + ", creationDate=" + this.getCreationDate() + ", owner=" + this.getOwner() + "]";
        }

        /**
         * Gets the bucket's owner.  Returns <code>null</code>
         * if the bucket's owner is unknown.
         */
        public Owner getOwner()
        {
            return this.owner;
        }

        /**
         * For internal use only.
         */
        public void setOwner(Owner owner)
        { 
            this.owner = owner;
        }

        /**
         * Gets the bucket's creation date. Returns <code>null</code>
         * if the creation date is not known.
         */
        public DateTime? getCreationDate()
        {
            return this.creationDate;
        }

        /**
         * For internal use only.
         */
        public void setCreationDate(DateTime? creationDate)
        {
            this.creationDate = creationDate;
        }

        /**
         * Gets the name of the bucket.
         */
        public String getName()
        {
            return this.name;
        }

        /**
         * Sets the name of the bucket. 
         * All buckets in KS3 share a single namespace;
         * ensure the bucket is given a unique name.
         */
        public void setName(String name)
        {
            this.name = name;
        }
    }
}
