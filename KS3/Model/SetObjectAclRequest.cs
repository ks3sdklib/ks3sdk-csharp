using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class SetObjectAclRequest : KS3Request
    {
        /** The name of the bucket whose object's ACL is being set. */
        private String bukcetName;

        /** The key of the object whose ACL is being set. */
        private String key;

        /** The custom ACL to apply to the specified object. */
        private AccessControlList acl;

        /** The canned ACL to apply to the specified object. */
        private CannedAccessControlList cannedAcl;

        /**
         * Constructs a new SetObjectAclRequest object, ready to set the specified
         * ACL on the specified object when this request is executed.
         */
        public SetObjectAclRequest(String bucketName, String key, AccessControlList acl)
        {
            this.bukcetName = bucketName;
            this.key = key;
            this.acl = acl;
            this.cannedAcl = null;
        }

        /**
         * Constructs a new SetObjectAclRequest object, ready to set the specified
         * ACL on the specified object when this request is executed.
         */
        public SetObjectAclRequest(String bucketName, String key, CannedAccessControlList cannedAcl)
        {
            this.bukcetName = bucketName;
            this.key = key;
            this.acl = null;
            this.cannedAcl = cannedAcl;
        }

	    /**
	     * Returns the name of the bucket whose object's ACL is being set.
	     */
        public String getBucketName()
        {
            return this.bukcetName;
        }

	    /**
	     * Returns the key of the object whose ACL is being setting.
	     */
        public String getKey()
        {
            return this.key;
        }

	    /**
	     * Returns the custom ACL to apply to the specified object.
	     */
        public AccessControlList getAcl()
        {
            return this.acl;
        }

	    /**
	     * Returns the canned ACL to apply to the specified object.
	     */
        public CannedAccessControlList getCannedAcl()
        {
            return this.cannedAcl;
        }
    }
}
