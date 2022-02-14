namespace KS3.Model
{
    /// <summary>
    /// Specifies constants defining a canned access control list.
    /// </summary>
    public class CannedAccessControlList
    {
        public static string PUBLICK_READ_WRITE = "public-read-write";
        public static string PUBLICK_READ = "public-read";
        public static string PRIVATE = "private";

        /// <summary>
        /// The KS3 x-kss-acl header value representing the canned acl
        /// </summary>
        public string CannedAclHeader { get; set; }

        public CannedAccessControlList(string cannedAclHeader)
        {
            CannedAclHeader = cannedAclHeader;
        }
 
        public override string ToString()
        {
            return CannedAclHeader;
        }
    }
}
