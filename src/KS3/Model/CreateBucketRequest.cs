using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    /**
     * Provides options for creating an KS3 bucket.
     */
    public class CreateBucketRequest : KS3Request
    {
        /** The name of the KS3 bucket to create. */
        private String bucketName;

        /**
	     * An optional access control list to apply to the new object. If specified,
	     * cannedAcl will be ignored.
	     */
        private AccessControlList acl;

        /**
	     * The optional Canned ACL to set for the new bucket. Ignored in favor of
	     * accessControlList, if present
	     */
        private CannedAccessControlList cannedAcl;

        public CreateBucketRequest(String bucketName)
        {
            this.bucketName = bucketName;
        }

        /**
         * Sets the name of the KS3 bucket to create.
         */
        public void setBucketName(String bucketName)
        {
            this.bucketName = bucketName;
        }

        /**
         * Gets the name of the KS3 bucket to create.
         */
        public String getBucketName()
        {
            return this.bucketName;
        }

        /**
         * Returns the optional access control list for the new bucket.
         */
        public CannedAccessControlList getCannedAcl()
        {
            return this.cannedAcl;
        }

        /**
         * Sets the optional access control list for the new bucket. If specified,
         * cannedAcl will be ignored.
         */
        public void setCannedAcl(CannedAccessControlList cannedAcl)
        {
            this.cannedAcl = cannedAcl;
        }

        /**
         * Returns the optional access control list for the new bucket. If
         * specified, cannedAcl will be ignored.
         */
        public AccessControlList getAcl()
        {
            return this.acl;
        }

        /**
         * Sets the optional access control list for the new bucket. If specified,
         * cannedAcl will be ignored.
         */
        public void setAcl(AccessControlList acl)
        {
            this.acl = acl;
        }
    }
}
