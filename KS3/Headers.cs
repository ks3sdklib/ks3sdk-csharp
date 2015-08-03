using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3
{
    public static class Headers
    {
        public static String CACHE_CONTROL = "Cache-Control";
        public static String CONTENT_DISPOSITION = "Content-Disposition";
        public static String CONTENT_ENCODING = "Content-Encoding";
        public static String CONTENT_LENGTH = "Content-Length";
        public static String CONTENT_MD5 = "Content-MD5";
        public static String CONTENT_TYPE = "Content-Type";
        public static String DATE = "Date";
        public static String ETAG = "ETag";
        public static String LAST_MODIFIED = "Last-Modified";
        public static String SERVER = "Server";
        public static String USER_AGENT = "User-Agent";
        public static String HOST = "Host";
        public static String CONNECTION = "Connection";

        /** Prefix for general KS3 headers: x-kss- */
        public static String KS3_PREFIX = "x-kss-";

        /** KS3's canned ACL header: x-amz-acl */
        public static String KS3_CANNED_ACL = "x-kss-acl";

        /** KS3's alternative date header: x-kss-date */
        public static String KS3_ALTERNATE_DATE = "x-kss-date";

        /** Prefix for KS3 user metadata: x-kss-meta- */
        public static String KS3_USER_METADATA_PREFIX = "x-kss-meta-";

        /** KS3 response header for a request's request ID */
        public static String REQUEST_ID = "x-kss-request-id";

        /** Range header for the get object request */
        public static String RANGE = "Range";

        /** Modified since constraint header for the get object request */
        public static String GET_OBJECT_IF_MODIFIED_SINCE = "If-Modified-Since";

        /** Unmodified since constraint header for the get object request */
        public static String GET_OBJECT_IF_UNMODIFIED_SINCE = "If-Unmodified-Since";

        /** ETag matching constraint header for the get object request */
        public static String GET_OBJECT_IF_MATCH = "If-Match";

        /** ETag non-matching constraint header for the get object request */
        public static String GET_OBJECT_IF_NONE_MATCH = "If-None-Match";

        /** Header for optional redirect location of an object */
        public static String REDIRECT_LOCATION = "x-kss-website-redirect-location";

        /** Header for the FULL_CONTROL permission */
        public static String PERMISSION_FULL_CONTROL = "x-kss-grant-full-control";

        /** Header for the READ permission */
        public static String PERMISSION_READ = "x-kss-grant-read";

        /** Header for the WRITE permission */
        public static String PERMISSION_WRITE = "x-kss-grant-write";

        /** Header for the READ_ACP permission */
        public static String PERMISSION_READ_ACP = "x-kss-grant-read-acp";

        /** Header for the WRITE_ACP permission */
        public static String PERMISSION_WRITE_ACP = "x-kss-grant-write-acp";
        /** Header for the copy object*/
        public static String XKssCopySource = "x-kss-copy-source";

        /**callback */
        public static String AsynchronousProcessingList = "kss-async-process";
        public static String NotifyURL = "kss-notifyurl";
        public static String TaskId = "TaskID";
    }
}
