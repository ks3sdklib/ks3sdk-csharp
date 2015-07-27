using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    /**
     * Represents an KS3 Access Control List (ACL), including the ACL's set of
     * grantees and the permissions assigned to each grantee.
     */
    public class AccessControlList
    {
        private ISet<Grant> grants = new HashSet<Grant>();
        private Owner owner = null;

        /**
         * Gets the owner of the AccessControlList.
         */
        public Owner getOwner()
        {
            return owner;
        }

        /**
         * For internal use only. Sets the owner on this access control list (ACL).
         * This method is only intended for internal use by the library.
         */
        public void setOwner(Owner owner)
        {
            this.owner = owner;
        }

        /**
         * Adds a grantee to the access control list (ACL) with the given permission. 
         * If this access control list already
         * contains the grantee (i.e. the same grantee object) the permission for the
         * grantee will be updated.
         */
        public void grantPermission(Grantee grantee, String permission)
        {
            this.grants.Add(new Grant(grantee, permission));
        }

        /**
         * Adds a set of grantee/permission pairs to the access control list (ACL), where each item in the
         * set is a Gran object.
         */
        public void grantAllPermissions(IList<Grant> grantList)
        {
            foreach (Grant grant in grantList)
                this.grantPermission(grant.getGrantee(), grant.getPermission());
        }

        /**
         * Revokes the permissions of a grantee by removing the grantee from the access control list (ACL).
         */
        public void revokeAllPermissions(Grantee grantee)
        {
            IList<Grant> grantsToRemove = new List<Grant>();
            foreach (Grant grant in grants)
                if (grant.getGrantee().Equals(grantee))
                    grantsToRemove.Add(grant);

            foreach (Grant grant in grantsToRemove)
                grants.Remove(grant);
        }

        /**
         * Gets the set of Grant objects in this access control list (ACL).
         */
        public ISet<Grant> getGrants()
        {
            return grants;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("AccessControlList:");
            builder.Append("\nOwner:\n" + this.owner);
            builder.Append("\nGrants:");

            foreach (Grant grant in grants)
                builder.Append("\n" + grant);

            return builder.ToString();
        }
    }
}
