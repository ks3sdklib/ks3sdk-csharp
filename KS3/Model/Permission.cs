using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public static class Permission
    {
        /**
         * Provides READ, WRITE, READ_ACP, and WRITE_ACP permissions.
         * It does not convey additional rights and is provided only for
         * convenience.
         */
        public static String FULL_CONTROL = "FULL_CONTROL";

        /**
         * Grants permission to list the bucket when applied to a bucket.
         * Grants permission to read object data
         * and/or metadata when applied to an object.
         */
        public static String READ = "READ";

        /**
         * Grants permission to create, overwrite, and
         * delete any objects in the bucket.
         * This permission is not supported for objects.
         */
        public static String WRITE = "WRITE";

        /**
         * Grants permission to read the ACL for the applicable bucket or object.
         * The owner of a bucket or object always implicitly has this permission.
         */
        public static String READ_ACP = "READ_ACP";

        /**
         * Gives permission to overwrite the ACP for the applicable bucket or
         * object.
         * The owner of a bucket or object always has this permission implicitly.
         * Granting this permission is equivalent to granting <code>FULL_CONTROL</code>because
         * the grant recipient can make any changes to the ACP.
         */
        public static String WRITE_ACP = "WRITE_ACP";

        /** A dictionary to find the headers for the perssions. */
        private static IDictionary<String, String> headers = new Dictionary<String, String>(){
            {FULL_CONTROL, Headers.PERMISSION_FULL_CONTROL},
            {READ, Headers.PERMISSION_READ},
            {WRITE, Headers.PERMISSION_WRITE},
            {READ_ACP, Headers.PERMISSION_READ_ACP},
            {WRITE_ACP, Headers.PERMISSION_WRITE_ACP}};

        /**
         * Returns the name of the header used to grant this permission.
         */
        public static String getHeaderName(String permissionString)
        {
            return headers[permissionString];
        }

        /**
         * Returns a list of the permissions.
         */
        public static IList<String> listPermissions()
        {
            return new List<String> { FULL_CONTROL, READ, WRITE, READ_ACP, WRITE_ACP };
        }
    }
}
