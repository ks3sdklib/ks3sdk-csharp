using KS3.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KS3.Model
{
    public class DefaultRequest<T> : IRequest<T>
    {
        /// <summary>
        /// Map of the parameters being sent as part of this request
        /// </summary>
        public IDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// Map of the headers included in this request
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// The resource path being requested
        /// </summary>
        public string ResourcePath { get; set; }

        /// <summary>
        /// The service endpoint to which this request should be sent
        /// </summary>
        public Uri Endpoint { get; set; }

        /// <summary>
        ///  The original, user facing request object which this internal request object is representing
        /// </summary>
        public KS3Request OriginalRequest { get; set; }

        /// <summary>
        /// The HTTP method to use when sending this request.
        /// </summary>
        public HttpMethod HttpMethod { get; set; }

        /// <summary>
        /// An optional stream from which to read the request payload.
        /// </summary>
        public Stream Content { get; set; }

        /// <summary>
        /// An optional time offset to account for clock skew
        /// </summary>
        public int TimeOffset { get; set; }

        public DefaultRequest()
        {
            Headers = new Dictionary<string, string>();
            Parameters = new Dictionary<string, string>();
        }

        /// <summary>
        /// Constructs a new DefaultRequest with the specified original, user facing request object.
        /// </summary>
        /// <param name="originalRequest"></param>
        public DefaultRequest(KS3Request originalRequest) : this()
        {
            OriginalRequest = originalRequest;
        }

        /// <summary>
        /// Set header
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetHeader(string name, string value)
        {
            Headers[name] = value;
        }

        /// <summary>
        /// Set parameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetParameter(string name, string value)
        {
            Parameters[name] = value;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(HttpMethod.ToString() + " ");
            builder.Append(Endpoint.ToString() + " ");
            builder.Append("/" + (string.IsNullOrWhiteSpace(ResourcePath) ? "" : ResourcePath) + " ");

            if (Parameters.Count > 0)
            {
                builder.Append("Parameters: (");
                foreach (var key in Parameters.Keys)
                {
                    var value = Parameters[key];
                    builder.Append(key + ": " + value + ", ");
                }
                builder.Append(") ");
            }

            if (Headers.Count > 0)
            {
                builder.Append("Headers: (");
                foreach (var key in Headers.Keys)
                {
                    var value = Headers[key];
                    builder.Append(key + ": " + value + ", ");
                }
                builder.Append(") ");
            }

            return builder.ToString();
        }
    }
}
