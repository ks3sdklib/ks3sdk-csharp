using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    /**
     * Request object containing all the options for requesting an object's Access Control List (ACL).
     */
    public class GetObjectAclRequest : KS3Request
    {
        /** The name of the bucket whose object's ACL is being retrieved. */
        private String bucketName;

        /** The key of the object whose ACL is being retrieved. */
        private String key;

        /**
    	 * Constructs a new GetObjectAclRequest object, ready to retrieve the ACL
    	 * for the specified bucket when executed.
    	 */
        public GetObjectAclRequest(String bucketName, String key)
        {
            this.bucketName = bucketName;
            this.key = key;
        }

        /**
         * Returns the name of the bucket whose object's ACL will be retrieved by
         * this request, when executed.
         */
        public String getBucketName()
        {
            return this.bucketName;
        }

        /**
    	 * Returns the key of the object whose ACL will be retrieved by this
    	 * request, when executed.
    	 */
        public String getKey()
        {
            return this.key;
        }
    }
}
