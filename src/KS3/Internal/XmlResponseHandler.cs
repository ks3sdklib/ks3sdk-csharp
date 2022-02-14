using KS3.Http;
using KS3.Transform;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace KS3.Internal
{
    public class XmlResponseHandler<X> : IHttpResponseHandler<X>
    {
        /// <summary>
        /// The SAX unmarshaller to use when handling the response from KS3
        /// </summary>
        private readonly IUnmarshaller<X, Stream> _responseUnmarshaller;

        /// <summary>
        /// Response headers from the processed response
        /// </summary>
        private IDictionary<string, string> _responseHeaders;

        /// <summary>
        /// Constructs a new KS3 response handler that will use the specified SAX unmarshaller to turn the response into an object.
        /// </summary>
        /// <param name="responseUnmarshaller"></param>
        public XmlResponseHandler(IUnmarshaller<X, Stream> responseUnmarshaller)
        {
            _responseUnmarshaller = responseUnmarshaller;
            _responseHeaders = new Dictionary<string, string>();
        }

        public X Handle(HttpWebResponse response)
        {
            X result = default;
            _responseHeaders = new Dictionary<string, string>();

            foreach (var key in response.Headers.AllKeys)
            {
                _responseHeaders.Add(key, response.Headers[key]);
            }
            if (_responseUnmarshaller != null)
            {
                result = _responseUnmarshaller.Unmarshall(response.GetResponseStream());
            }
            return result;
        }

        /// <summary>
        ///  Returns the headers from the processed response. Will return null until a response has been handled.
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> GetResponseHeaders()
        {
            return _responseHeaders;
        }
    }
}
