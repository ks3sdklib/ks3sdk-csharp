using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

using KS3.Http;
using KS3.Transform;

namespace KS3.Internal
{
    public class XmlResponseHandler<X> : HttpResponseHandler<X>
    {
        /** The SAX unmarshaller to use when handling the response from KS3 */
        private Unmarshaller<X, Stream> responseUnmarshaller;

        /** Response headers from the processed response */
        private IDictionary<String, String> responseHeaders;


        /**
         * Constructs a new KS3 response handler that will use the specified SAX
         * unmarshaller to turn the response into an object.
         */
        public XmlResponseHandler(Unmarshaller<X, Stream> responseUnmarshaller)
        {
            this.responseUnmarshaller = responseUnmarshaller;
        }

        public X handle(HttpWebResponse response)
        {
            X result = default(X);
            responseHeaders = new Dictionary<String, String>();

            foreach (String key in response.Headers.AllKeys)
                responseHeaders.Add(key, response.Headers[key]);

            if (responseUnmarshaller != null)
                result = responseUnmarshaller.unmarshall(response.GetResponseStream());
            
            return result;
        }

        /**
         * Returns the headers from the processed response. Will return null until a
         * response has been handled.
         */
        public IDictionary<String, String> getResponseHeaders()
        {
            return this.responseHeaders;
        }
    }
}
