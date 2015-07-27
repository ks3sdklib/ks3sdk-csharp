using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    /**
     * Specifies a grant, consisting of one grantee and one permission.
     */
    public class Grant
    {
        private Grantee grantee = null;
        private String permission = null;

        /**
         * Constructs a new Grant object using the specified grantee and
         * permission objects.
         */
        public Grant(Grantee grantee, String permission)
        {
            this.grantee = grantee;
            this.permission = permission;
        }

	    /**
	     * Gets the grantee being granted a permission by this grant.
	     */
        public Grantee getGrantee()
        {
            return this.grantee;
        }

	    /**
	     * Gets the permission being granted to the grantee by this grant.
	     */
        public String getPermission()
        {
            return this.permission;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            
            if (this.GetType() == obj.GetType())
            {
                Grant other = (Grant)obj;
                if (this.permission.Equals(other.permission)
                    && this.grantee.Equals(other.grantee))
                    return true;
            }
            
            return false;
        }

        public override int GetHashCode()
        {
            return this.grantee.GetHashCode() * 419 + this.permission.GetHashCode();
        }

        public override string ToString()
        {
            return "Grant [grantee=" + this.grantee + ", permission=" + this.permission + "]";
        }
    }
}
