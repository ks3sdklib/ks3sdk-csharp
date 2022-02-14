using System;
using System.Collections.Generic;
using System.Text;

namespace KS3.Model
{
    public class ObjectMetadata
    {
        /// <summary>
        /// Custom user metadata, represented in responses with the x-kss-meta- header prefix
        /// </summary>
        private readonly IDictionary<string, string> _userMetadata = new Dictionary<string, string>();

        /// <summary>
        /// All other (non user custom) headers such as Content-Length, Content-Type, etc.
        /// </summary>
        private readonly IDictionary<string, object> _metadata = new Dictionary<string, object>();

        /// <summary>
        /// For internal use only. Gets a dictionary of the raw metadata/headers for the associated object.
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, object> GetRawMetadata()
        {
            return this._metadata;
        }

        /// <summary>
        /// For internal use only. Sets a specific metadata header value. Not intended to be called by external code.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetHeader(string key, object value)
        {
            _metadata[key] = value;
        }

        /// <summary>
        /// Sets the key value pair of custom user-metadata for the associated object. If the entry in the custom user-metadata map already contains the specified key, it will be replaced with these new contents.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetUserMetaData(string key, string value)
        {
            this._userMetadata[key] = value;
        }

        /// <summary>
        /// Gets the custom user-metadata for the associated object.
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> GetUserMetadata()
        {
            return this._userMetadata;
        }

        /// <summary>
        /// Gets the value of the Last-Modified header, indicating the date and time at which KS3 last recorded a modification to the associated object.
        /// </summary>
        /// <returns></returns>
        public DateTime? GetLastModified()
        {
            if (!_metadata.ContainsKey(Headers.LAST_MODIFIED))
            {
                return null;
            }
            return (DateTime)_metadata[Headers.LAST_MODIFIED];
        }

        /// <summary>
        /// For internal use only. Sets the Last-Modified header value indicating the date and time at which KS3 last recorded a modification to the associated object.
        /// </summary>
        /// <param name="lastModified"></param>
        public void SetLastModified(DateTime lastModified)
        {
            _metadata[Headers.LAST_MODIFIED] = lastModified;
        }

        /// <summary>
        /// Gets the Content-Length HTTP header indicating the size of the associated object in bytes.
        /// </summary>
        /// <returns></returns>
        public long GetContentLength()
        {
            if (!_metadata.ContainsKey(Headers.CONTENT_LENGTH))
            {
                return 0;
            }
            return (long)_metadata[Headers.CONTENT_LENGTH];
        }

        /// <summary>
        /// Sets the Content-Length HTTP header indicating the size of the associated object in bytes.
        /// </summary>
        /// <param name="contentLength"></param>
        public void SetContentLength(long contentLength)
        {
            _metadata[Headers.CONTENT_LENGTH] = contentLength;
        }

        /// <summary>
        /// Gets the Content-Type HTTP header, which indicates the type of content stored in the associated object. The value of this header is a standard MIME type.
        /// </summary>
        /// <returns></returns>
        public string GetContentType()
        {
            if (!_metadata.ContainsKey(Headers.CONTENT_TYPE))
            {
                return null;
            }
            return (string)_metadata[Headers.CONTENT_TYPE];
        }

        /// <summary>
        /// Sets the Content-Type HTTP header indicating the type of content stored in the associated object. The value of this header is a standard MIME type.
        /// </summary>
        /// <param name="contentType"></param>
        public void SetContentType(string contentType)
        {
            _metadata[Headers.CONTENT_TYPE] = contentType;
        }

        /// <summary>
        /// Gets the optional Content-Encoding HTTP header specifying what content encodings have been applied to the object and what decoding mechanisms must be applied in order to obtain the media-type referenced by the Content-Type field.
        /// </summary>
        /// <returns></returns>
        public string GetContentEncoding()
        {
            if (!_metadata.ContainsKey(Headers.CONTENT_ENCODING))
            {
                return null;
            }
            return (string)_metadata[Headers.CONTENT_ENCODING];
        }

        /// <summary>
        /// Sets the optional Content-Encoding HTTP header specifying what content encodings have been applied to the object and what decoding mechanisms must be applied in order to obtain the media-type referenced by the Content-Type field.
        /// </summary>
        /// <param name="encoding"></param>
        public void SetContentEncoding(string encoding)
        {
            _metadata[Headers.CONTENT_ENCODING] = encoding;
        }

        /// <summary>
        /// Sets the base64 encoded 128-bit MD5 digest of the associated object (content - not including headers) according to RFC 1864. This data is used as a message integrity check to verify that the data received by KS3 is the same data that the caller sent. If set to null,then the MD5 digest is removed from the metadata.
        /// </summary>
        /// <param name="md5Base64"></param>
        public void SetContentMD5(string md5Base64)
        {
            if (string.IsNullOrWhiteSpace(md5Base64))
            {
                _metadata.Remove(Headers.CONTENT_MD5);
            }
            else
            {
                _metadata[Headers.CONTENT_MD5] = md5Base64;
            }
        }

        /// <summary>
        /// Gets the base64 encoded 128-bit MD5 digest of the associated object (content - not including headers) according to RFC 1864. This data is used as a message integrity check to verify that the data received by KS3 is the same data that the caller sent.
        /// </summary>
        /// <returns></returns>
        public string GetContentMD5()
        {
            if (!_metadata.ContainsKey(Headers.CONTENT_MD5))
            {
                return string.Empty;
            }
            return (string)_metadata[Headers.CONTENT_MD5];
        }

        /// <summary>
        /// Gets the hex encoded 128-bit MD5 digest of the associated object according to RFC 1864. This data is used as an integrity check to verify that the data received by the caller is the same data that was sent by KS3.
        /// </summary>
        /// <returns></returns>
        public string GetETag()
        {
            if (!_metadata.ContainsKey(Headers.ETAG))
            {
                return string.Empty;
            }
            return (string)_metadata[Headers.ETAG];
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<metadata>");
            foreach (string name in this._metadata.Keys)
            {
                builder.Append("\n" + name + ": " + _metadata[name]);
            }
            builder.Append("\n</metadata>");

            builder.Append("\n<userMetadata>");
            foreach (string name in this._userMetadata.Keys)
            {
                builder.Append("\n" + name + ": " + _userMetadata[name]);
            }
            builder.Append("\n</userMetadata>");

            return builder.ToString();
        }
    }
}
