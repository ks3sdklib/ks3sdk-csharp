namespace KS3
{
    public static class Headers
    {
        public static string CACHE_CONTROL = "Cache-Control";
        public static string CONTENT_DISPOSITION = "Content-Disposition";
        public static string CONTENT_ENCODING = "Content-Encoding";
        public static string CONTENT_LENGTH = "Content-Length";
        public static string CONTENT_MD5 = "Content-MD5";
        public static string CONTENT_TYPE = "Content-Type";
        public static string DATE = "Date";
        public static string ETAG = "ETag";
        public static string LAST_MODIFIED = "Last-Modified";
        public static string SERVER = "Server";
        public static string USER_AGENT = "User-Agent";
        public static string HOST = "Host";
        public static string CONNECTION = "Connection";

        /// <summary>
        /// Prefix for general KS3 headers: x-kss-
        /// </summary>
        public static string KS3_PREFIX = "x-kss-";

        /// <summary>
        /// KS3's canned ACL header: x-amz-acl
        /// </summary>
        public static string KS3_CANNED_ACL = "x-kss-acl";

        /// <summary>
        /// KS3's alternative date header: x-kss-date
        /// </summary>
        public static string KS3_ALTERNATE_DATE = "x-kss-date";

        /// <summary>
        /// Prefix for KS3 user metadata: x-kss-meta-
        /// </summary>
        public static string KS3_USER_METADATA_PREFIX = "x-kss-meta-";

        /// <summary>
        /// KS3 response header for a request's request ID 
        /// </summary>
        public static string REQUEST_ID = "x-kss-request-id";

        /// <summary>
        ///  Range header for the get object request
        /// </summary>
        public static string RANGE = "Range";

        /// <summary>
        /// Modified since constraint header for the get object request
        /// </summary>
        public static string GET_OBJECT_IF_MODIFIED_SINCE = "If-Modified-Since";

        /// <summary>
        /// Unmodified since constraint header for the get object request 
        /// </summary>
        public static string GET_OBJECT_IF_UNMODIFIED_SINCE = "If-Unmodified-Since";

        /// <summary>
        /// ETag matching constraint header for the get object request
        /// </summary>
        public static string GET_OBJECT_IF_MATCH = "If-Match";

        /// <summary>
        /// ETag non-matching constraint header for the get object request 
        /// </summary>
        public static string GET_OBJECT_IF_NONE_MATCH = "If-None-Match";

        /// <summary>
        /// Header for optional redirect location of an object
        /// </summary>
        public static string REDIRECT_LOCATION = "x-kss-website-redirect-location";

        /// <summary>
        ///  Header for the FULL_CONTROL permission
        /// </summary>
        public static string PERMISSION_FULL_CONTROL = "x-kss-grant-full-control";

        /// <summary>
        /// Header for the READ permission
        /// </summary>
        public static string PERMISSION_READ = "x-kss-grant-read";

        /// <summary>
        /// Header for the WRITE permission
        /// </summary>
        public static string PERMISSION_WRITE = "x-kss-grant-write";

        /// <summary>
        /// Header for the READ_ACP permission
        /// </summary>
        public static string PERMISSION_READ_ACP = "x-kss-grant-read-acp";

        /// <summary>
        /// Header for the WRITE_ACP permission
        /// </summary>
        public static string PERMISSION_WRITE_ACP = "x-kss-grant-write-acp";

        /// <summary>
        /// Header for the copy object
        /// </summary>
        public static string XKssCopySource = "x-kss-copy-source";

        /**callback **/
        public static string AsynchronousProcessingList = "kss-async-process";
        public static string NotifyURL = "kss-notifyurl";
        public static string TaskId = "TaskID";
    }
}
