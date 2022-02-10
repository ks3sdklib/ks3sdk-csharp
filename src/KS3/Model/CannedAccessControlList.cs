using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    /**
     * Specifies constants defining a canned access control list.
     */
    public class CannedAccessControlList
    {
        public static String PUBLICK_READ_WRITE = "public-read-write";
        public static String PUBLICK_READ = "public-read";
        public static String PRIVATE = "private";

        /** The KS3 x-kss-acl header value representing the canned acl */
        private String cannedAclHeader;

        public CannedAccessControlList(String cannedAclHeader)
        {
            this.cannedAclHeader = cannedAclHeader;
        }

        /**
         * Returns the KS3 x-kss-acl header value for this canned acl.
         */
        public String getCannedAclHeader()
        {
            return this.cannedAclHeader;
        }

        public override String ToString()
        {
            return this.cannedAclHeader;
        }
    }
}
