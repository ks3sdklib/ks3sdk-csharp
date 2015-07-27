using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class ObjectMetadata
    {
        /**
         * Custom user metadata, represented in responses with the x-kss-meta-
         * header prefix
         */
        private IDictionary<String, String> userMetadata = new Dictionary<String, String>();

        /**
         * All other (non user custom) headers such as Content-Length, Content-Type,
         * etc.
         */
        private IDictionary<String, Object> metadata = new Dictionary<String, Object>();

        /**
         * For internal use only. Gets a dictionary of the raw metadata/headers
         * for the associated object.
         */
        public IDictionary<String, Object> getRawMetadata()
        {
            return this.metadata;
        }

        /**
         * For internal use only. Sets a specific metadata header value. Not
         * intended to be called by external code.
         */
        public void setHeader(String key, Object value)
        {
            metadata[key] = value;
        }

        /**
         * Sets the key value pair of custom user-metadata for the associated
         * object. If the entry in the custom user-metadata map already contains the
         * specified key, it will be replaced with these new contents.
         */
        public void setUserMetaData(String key, String value)
        {
            this.userMetadata[key] = value;
        }

        /**
         * Gets the custom user-metadata for the associated object.
         */
        public IDictionary<String, String> getUserMetadata()
        {
            return this.userMetadata;
        }

        /**
         * Gets the value of the Last-Modified header, indicating the date
         * and time at which KS3 last recorded a modification to the
         * associated object.
         */
        public DateTime? getLastModified()
        {
            if (!metadata.ContainsKey(Headers.LAST_MODIFIED)) return null;
            return (DateTime)metadata[Headers.LAST_MODIFIED];
        }

        /**
         * For internal use only. Sets the Last-Modified header value
         * indicating the date and time at which KS3 last recorded a
         * modification to the associated object.
         */
        public void setLastModified(DateTime lastModified)
        {
            metadata[Headers.LAST_MODIFIED] = lastModified;
        }

        /**
         * Gets the Content-Length HTTP header indicating the size of the
         * associated object in bytes.
         */
        public long getContentLength()
        {
            if (!metadata.ContainsKey(Headers.CONTENT_LENGTH)) return 0;
            return (long)metadata[Headers.CONTENT_LENGTH];
        }

        /**
         * Sets the Content-Length HTTP header indicating the size of the
         * associated object in bytes.
         */
        public void setContentLength(long contentLength)
        {
            metadata[Headers.CONTENT_LENGTH] = contentLength;
        }

        /**
         * Gets the Content-Type HTTP header, which indicates the type of content
         * stored in the associated object. The value of this header is a standard
         * MIME type.
         */
        public String getContentType()
        {
            if (!metadata.ContainsKey(Headers.CONTENT_TYPE)) return null;
            return (String)metadata[Headers.CONTENT_TYPE];
        }

        /**
         * Sets the Content-Type HTTP header indicating the type of content
         * stored in the associated object. The value of this header is a standard
         * MIME type.
         */
        public void setContentType(String contentType)
        {
            metadata[Headers.CONTENT_TYPE] = contentType;
        }

        /**
         * Gets the optional Content-Encoding HTTP header specifying what
         * content encodings have been applied to the object and what decoding
         * mechanisms must be applied in order to obtain the media-type referenced
         * by the Content-Type field.
         */
        public String getContentEncoding()
        {
            if (!metadata.ContainsKey(Headers.CONTENT_ENCODING)) return null;
            return (String)metadata[Headers.CONTENT_ENCODING];
        }

        /**
         * Sets the optional Content-Encoding HTTP header specifying what
         * content encodings have been applied to the object and what decoding
         * mechanisms must be applied in order to obtain the media-type referenced
         * by the Content-Type field.
         */
        public void setContentEncoding(String encoding)
        {
            metadata[Headers.CONTENT_ENCODING] = encoding;
        }

        /**
	     * Sets the base64 encoded 128-bit MD5 digest of the associated object
	     * (content - not including headers) according to RFC 1864. This data is
	     * used as a message integrity check to verify that the data received by
	     * KS3 is the same data that the caller sent. If set to null,then the
	     * MD5 digest is removed from the metadata.
	     */
        public void setContentMD5(String md5Base64)
        {
            if (md5Base64 == null) metadata.Remove(Headers.CONTENT_MD5);
            else metadata[Headers.CONTENT_MD5] = md5Base64;
        }

        /**
         * Gets the base64 encoded 128-bit MD5 digest of the associated object
         * (content - not including headers) according to RFC 1864. This data is
         * used as a message integrity check to verify that the data received by
         * KS3 is the same data that the caller sent.
         */
        public String getContentMD5()
        {
            if (!metadata.ContainsKey(Headers.CONTENT_MD5)) return null;
            return (String)metadata[Headers.CONTENT_MD5];
        }

        /**
         * Gets the hex encoded 128-bit MD5 digest of the associated object
         * according to RFC 1864. This data is used as an integrity check to verify
         * that the data received by the caller is the same data that was sent by
         * KS3.
         */
        public String getETag()
        {
            if (!metadata.ContainsKey(Headers.ETAG)) return null;
            return (String)metadata[Headers.ETAG];
        }

        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<metadata>");
            foreach(String name in this.metadata.Keys)
                builder.Append("\n" + name + ": " + metadata[name]);
            builder.Append("\n</metadata>");

            builder.Append("\n<userMetadata>");
            foreach(String name in this.userMetadata.Keys)
                builder.Append("\n" + name + ": " + userMetadata[name]);
            builder.Append("\n</userMetadata>");

            return builder.ToString();
        }
    }
}
