using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    /**
     * Request object containing all the options for setting a bucket's Access Control List (ACL).
     */
    public class SetBucketAclRequest : KS3Request
    {
        /** The name of the bucket whose ACL is being set. */
        private String bukcetName;

        /** The custom ACL to apply to the specified bucket. */
        private AccessControlList acl;

        /** The canned ACL to apply to the specified bucket. */
        private CannedAccessControlList cannedAcl;
        
        /**
         * Constructs a new SetBucketAclRequest object, ready to set the specified
         * ACL on the specified bucket when this request is executed.
         */
        public SetBucketAclRequest(String bucketName, AccessControlList acl)
        {
            this.bukcetName = bucketName;
            this.acl = acl;
            this.cannedAcl = null;
        }

	    /**
	     * Constructs a new SetBucketAclRequest object, ready to set the specified
	     * canned ACL on the specified bucket when this request is executed.
	     */
        public SetBucketAclRequest(String bucketName, CannedAccessControlList cannedAcl)
        {
            this.bukcetName = bucketName;
            this.acl = null;
            this.cannedAcl = cannedAcl;
        }

	    /**
	     * Returns the name of the bucket whose ACL will be modified by this request
	     * when executed.
         */
        public String getBucketName()
        {
            return this.bukcetName;
        }

	    /**
	     * Returns the custom ACL to be applied to the specified bucket when this
	     * request is executed. A request can use either a custom ACL or a canned
	     * ACL, but not both.
	     */
        public AccessControlList getAcl()
        {
            return this.acl;
        }

	    /**
	     * Returns the canned ACL to be applied to the specified bucket when this
	     * request is executed. A request can use either a custom ACL or a canned
	     * ACL, but not both.
	     */
        public CannedAccessControlList getCannedAcl()
        {
            return this.cannedAcl;
        }
    }
}
