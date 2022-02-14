using System;
using System.Collections.Generic;
using System.Text;

namespace KS3.Model
{
    /// <summary>
    /// Represents an KS3 Access Control List (ACL), including the ACL's set of grantees and the permissions assigned to each grantee.
    /// </summary>
    public class AccessControlList
    {
        public ISet<Grant> Grants { get; set; } = new HashSet<Grant>();

        public Owner Owner { get; set; }

        /// <summary>
        /// Adds a grantee to the access control list (ACL) with the given permission.  If this access control list already contains the grantee (i.e. the same grantee object) the permission for the grantee will be updated.
        /// </summary>
        /// <param name="grantee"></param>
        /// <param name="permission"></param>
        public void GrantPermission(IGrantee grantee, String permission)
        {
            Grants.Add(new Grant(grantee, permission));
        }

        /// <summary>
        ///  Adds a set of grantee/permission pairs to the access control list (ACL), where each item in the set is a Gran object.
        /// </summary>
        /// <param name="grantList"></param>
        public void GrantAllPermissions(IList<Grant> grantList)
        {
            foreach (Grant grant in grantList)
            {
                GrantPermission(grant.Grantee, grant.Permission);
            }
        }

        /// <summary>
        /// Revokes the permissions of a grantee by removing the grantee from the access control list (ACL).
        /// </summary>
        /// <param name="grantee"></param>
        public void RevokeAllPermissions(IGrantee grantee)
        {
           var grantsToRemove = new List<Grant>();
            foreach (Grant grant in Grants)
            {
                if (grant.Grantee.Equals(grantee))
                {
                    grantsToRemove.Add(grant);
                }
            }

            foreach (Grant grant in grantsToRemove)
            {
                Grants.Remove(grant);
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("AccessControlList:");
            builder.Append($"\nOwner:\n{Owner}");
            builder.Append("\nGrants:");

            foreach (Grant grant in Grants)
            {
                builder.Append("\n" + grant);
            }

            return builder.ToString();
        }
    }
}
