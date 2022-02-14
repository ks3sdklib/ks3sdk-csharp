namespace KS3.Model
{
    public class ResponseHeaderOverrides
    {
        internal const string RESPONSE_CONTENT_TYPE = "response-content-type";
        internal const string RESPONSE_CONTENT_LANGUAGE = "response-content-language";
        internal const string RESPONSE_EXPIRES = "response-expires";
        internal const string RESPONSE_CACHE_CONTROL = "response-cache-control";
        internal const string RESPONSE_CONTENT_DISPOSITION = "response-content-disposition";
        internal const string RESPONSE_CONTENT_ENCODING = "response-content-encoding";

        /// <summary>
        /// A standard MIME type describing the format of the object data.
        /// </summary>
        /// <remarks>
        /// The content type for the content being uploaded. This property defaults to "binary/octet-stream".
        /// For more information, refer to: <see href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.17"/>
        /// </remarks>
        public string ContentType { get; set; }

        /// <summary>
        /// ContentLanguage header value.
        /// </summary>
        public string ContentLanguage { get; set; }

        /// <summary>
        /// Expiry header value.
        /// </summary>
        public string Expires { get; set; }

        /// <summary>
        /// CacheControl header value.
        /// </summary>
        public string CacheControl { get; set; }

        /// <summary>
        /// The ContentDisposition header value.
        /// </summary>
        public string ContentDisposition { get; set; }

        /// <summary>
        /// The ContentEncoding header value.
        /// </summary>
        public string ContentEncoding { get; set; }
    }
}
