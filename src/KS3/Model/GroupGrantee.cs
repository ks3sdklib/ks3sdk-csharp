using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    /// <summary>
    /// Specifies constants defining a group of KS3 users who can be granted permissions to KS3 buckets and objects.
    /// </summary>
    public class GroupGrantee : IGrantee
    {
        /// <summary>
        /// Grants anonymous access to any KS3 object or bucket. Any user will be able to access the object by omitting the KS3 Key ID and Signature from a request.
        /// </summary>
        public static string ALL_USERS = "http://acs.ksyun.com/groups/global/AllUsers";

        private string _groupUri;

        public GroupGrantee(string groupUri)
        {
            _groupUri = groupUri;
        }

        public string GetTypeIdentifier()
        {
            return "uri";
        }

        /// <summary>
        /// Gets the group grantee's URI.
        /// </summary>
        /// <returns></returns>
        public string GetIdentifier()
        {
            return this._groupUri;
        }

        /// <summary>
        /// For internal use only. Group grantees have preset identifiers that cannot be modified.
        /// </summary>
        /// <param name="id"></param>
        public void SetIdentifier(string id)
        {
            _groupUri = id;
        }

        public override int GetHashCode()
        {
            return _groupUri.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() == this.GetType())
            {
                GroupGrantee other = (GroupGrantee)obj;
                return _groupUri.Equals(other._groupUri);
            }

            return false;
        }

        public override string ToString()
        {
            return $"GroupGrantee [{_groupUri}]";
        }
    }
}
