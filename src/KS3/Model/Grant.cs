namespace KS3.Model
{
    /// <summary>
    /// Specifies a grant, consisting of one grantee and one permission.
    /// </summary>
    public class Grant
    {
        public IGrantee Grantee { get; set; }
        public string Permission { get; set; }

        /// <summary>
        /// Constructs a new Grant object using the specified grantee and permission objects.
        /// </summary>
        /// <param name="grantee"></param>
        /// <param name="permission"></param>
        public Grant(IGrantee grantee, string permission)
        {
            Grantee = grantee;
            Permission = permission;
        }


        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (this.GetType() == obj.GetType())
            {
                Grant other = (Grant)obj;
                if (Permission.Equals(other.Permission) && Grantee.Equals(other.Grantee))
                {
                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Grantee.GetHashCode() * 419 + Permission.GetHashCode();
        }

        public override string ToString()
        {
            return $"Grant [grantee={Grantee}, permission={Permission}]";
        }
    }
}
