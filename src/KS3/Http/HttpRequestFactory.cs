using KS3.Extensions;
using KS3.Internal;
using KS3.Model;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace KS3.Http
{
    public static class HttpRequestFactory
    {
        /// <summary>
        /// Creates an HttpWebRequest based on the specified request and populates any parameters, headers, etc. from the original request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="clientConfiguration"></param>
        /// <returns></returns>
        public static HttpWebRequest CreateHttpRequest<T>(
            IRequest<T> request,
            ClientConfiguration clientConfiguration) where T : KS3Request
        {
            var uri = request.Endpoint.ToString();
            if (request.ResourcePath != null && request.ResourcePath.Length > 0)
            {
                if (request.ResourcePath.StartsWith("/"))
                {
                    if (uri.EndsWith("/")) uri = uri.Substring(0, uri.Length - 1);
                }
                else if (!uri.EndsWith("/"))
                {
                    uri += "/";
                }
                uri += request.ResourcePath;
            }
            else if (!uri.EndsWith("/"))
            {
                uri += "/";
            }
            var encodedParams = EncodeParameters(request);

            //For all non-POST requests, and any POST requests that already have a payload, we put the encoded params directly in the URI, otherwise,we'll put them in the POST request's payload.
            bool putParamsInUri = request.HttpMethod != HttpMethod.POST || request.Content != null;

            if (encodedParams != null && (putParamsInUri || encodedParams.Contains("upload")))
                uri += "?" + encodedParams;

            if (request.HttpMethod == HttpMethod.POST && encodedParams != null && !putParamsInUri && !encodedParams.Contains("upload"))
                request.Content = (new MemoryStream(Constants.DEFAULT_ENCODING.GetBytes(encodedParams)));
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpRequest.Method = request.HttpMethod.ToString();

            httpRequest.AllowWriteStreamBuffering = false; // important

            httpRequest.Timeout = clientConfiguration.Timeout;
            httpRequest.ReadWriteTimeout = clientConfiguration.ReadWriteTimeout;

            ConfigureHeaders(httpRequest, request, clientConfiguration);

            if (request.Content != null)
            {
                Stream inputStream = request.Content;
                if (inputStream.CanSeek)
                {
                    inputStream.Seek(0, SeekOrigin.Begin);
                }
                Stream requestStream = httpRequest.GetRequestStream();
                int bufferSize = Constants.DEFAULT_STREAM_BUFFER_SIZE;
                byte[] buf = new byte[bufferSize];

                for (; ; )
                {
                    int size = inputStream.Read(buf, 0, Constants.DEFAULT_STREAM_BUFFER_SIZE);
                    if (size <= 0)
                    {
                        break;
                    }
                    requestStream.Write(buf, 0, size);
                }

                requestStream.Flush();
                requestStream.Close();
            }

            return httpRequest;
        }

        /// <summary>
        /// Creates an encoded query string from all the parameters in the specified request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string EncodeParameters<T>(IRequest<T> request) where T : KS3Request
        {
            if (request.Parameters.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            bool first = true;
            char separator = '&';

            foreach (var name in request.Parameters.Keys)
            {
                var value = request.Parameters[name];
                if (!first)
                {
                    builder.Append(separator);
                }
                else
                {
                    first = false;
                }


                //builder.Append(name + (value != null ? ("=" + value) : ""));
                var v = value.IsNullOrWhiteSpace() ? "" : $"={value}";
                builder.Append($"{name}{v}");
            }

            return builder.ToString();
        }

        /// <summary>
        /// Configures the headers in the specified HTTP request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpRequest"></param>
        /// <param name="request"></param>
        /// <param name="clientConfiguration"></param>
        private static void ConfigureHeaders<T>(
            HttpWebRequest httpRequest,
            IRequest<T> request,
            ClientConfiguration clientConfiguration) where T : KS3Request
        {
            // Copy over any other headers already in our request
            foreach (var name in request.Headers.Keys)
            {
                if (name.Equals(Headers.HOST))
                {
                    continue;
                }

                var value = request.Headers[name];

                if (name.Equals(Headers.CONTENT_TYPE))
                {
                    httpRequest.ContentType = value;
                }
                else if (name.Equals(Headers.CONTENT_LENGTH))
                {
                    httpRequest.ContentLength = long.Parse(value);
                }
                else if (name.Equals(Headers.USER_AGENT))
                {
                    httpRequest.UserAgent = value;
                }
                else if (name.Equals(Headers.DATE))
                {
                    httpRequest.Date = DateTime.Parse(value);
                }
                else if (name.Equals(Headers.RANGE))
                {
                    var range = value.Split('-');
                    httpRequest.AddRange(long.Parse(range[0]), long.Parse(range[1]));
                }
                else if (name.Equals(Headers.GET_OBJECT_IF_MODIFIED_SINCE))
                {
                    httpRequest.IfModifiedSince = DateTime.Parse(value);
                }
                else
                {
                    httpRequest.Headers[name] = value;
                }
            }

            //Set content type and encoding
            if (!httpRequest.Headers.AllKeys.Contains(Headers.CONTENT_TYPE) || httpRequest.Headers[Headers.CONTENT_TYPE].Length == 0)
            {
                httpRequest.ContentType = Mimetypes.DEFAULT_MIMETYPE;
            }
        }

    }
}
