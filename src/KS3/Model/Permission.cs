using System.Collections.Generic;

namespace KS3.Model
{
    public static class Permission
    {
        /// <summary>
        ///  * Provides READ, WRITE, READ_ACP, and WRITE_ACP permissions.
        ///  * It does not convey additional rights and is provided only for
        ///  * convenience.
        /// </summary>
        public static string FULL_CONTROL = "FULL_CONTROL";

        /// <summary>
        ///  * Grants permission to list the bucket when applied to a bucket.
        ///  * Grants permission to read object data
        ///  * and/or metadata when applied to an object.
        /// </summary>
        public static string READ = "READ";

        /// <summary>
        /// * Grants permission to create, overwrite, and
        /// * delete any objects in the bucket.
        /// * This permission is not supported for objects.
        /// </summary>
        public static string WRITE = "WRITE";

        /// <summary>
        /// * Grants permission to read the ACL for the applicable bucket or object.
        /// * The owner of a bucket or object always implicitly has this permission.
        /// </summary>
        public static string READ_ACP = "READ_ACP";

        /// <summary>
        /// * Gives permission to overwrite the ACP for the applicable bucket or
        /// * object.
        /// * The owner of a bucket or object always has this permission implicitly.
        /// * Granting this permission is equivalent to granting<code> FULL_CONTROL</code>because
        /// * the grant recipient can make any changes to the ACP.
        /// </summary>
        public static string WRITE_ACP = "WRITE_ACP";

        /// <summary>
        /// A dictionary to find the headers for the perssions.
        /// </summary>
        private static readonly IDictionary<string, string> _headers = new Dictionary<string, string>(){

            {FULL_CONTROL, Headers.PERMISSION_FULL_CONTROL},
            {READ, Headers.PERMISSION_READ},
            {WRITE, Headers.PERMISSION_WRITE},
            {READ_ACP, Headers.PERMISSION_READ_ACP},
            {WRITE_ACP, Headers.PERMISSION_WRITE_ACP}
        };

        /// <summary>
        /// Returns the name of the header used to grant this permission.
        /// </summary>
        /// <param name="permissionString"></param>
        /// <returns></returns>
        public static string GetHeaderName(string permissionString)
        {
            return _headers[permissionString];
        }

        /// <summary>
        ///  Returns a list of the permissions.
        /// </summary>
        /// <returns></returns>
        public static IList<string> ListPermissions()
        {
            return new List<string> { FULL_CONTROL, READ, WRITE, READ_ACP, WRITE_ACP };
        }
    }
}
