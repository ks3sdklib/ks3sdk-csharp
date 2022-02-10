using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    /**
     * Specifies constants defining a group of KS3 users who can be granted
     * permissions to KS3 buckets and objects.
     */
    public class GroupGrantee : Grantee
    {
        /**
         * Grants anonymous access to any KS3 object or bucket. Any user will
         * be able to access the object by omitting the KS3 Key ID and Signature
         * from a request.
         */
        public static string ALL_USERS = "http://acs.ksyun.com/groups/global/AllUsers";

        private String groupUri;

        public GroupGrantee(String groupUri)
        {
            this.groupUri = groupUri;
        }

        public String getTypeIdentifier()
        {
            return "uri";
        }

        /**
         * Gets the group grantee's URI.
         */
        public String getIdentifier()
        {
            return this.groupUri;
        }

        /**
         * For internal use only. Group grantees have preset identifiers that cannot
         * be modified.
         */
        public void setIdentifier(String id)
        {
            this.groupUri = id;
        }

        public override int GetHashCode()
        {
            return this.groupUri.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj.GetType() == this.GetType())
            {
                GroupGrantee other = (GroupGrantee)obj;
                return other.groupUri.Equals(other.groupUri);
            }

            return false;
        }

        public override string ToString()
        {
            return "GroupGrantee [" + this.groupUri + "]";
        }
    }
}
