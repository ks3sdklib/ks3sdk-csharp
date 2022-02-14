using KS3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace KS3.Internal
{
    public static class RestUtils
    {
        private static readonly IList<string> SIGNED_PARAMETERS = new List<string> {
            "acl","adp","torrent", "logging", "location", "policy", "requestPayment", "versioning",
            "versions", "versionId", "notification", "uploadId", "uploads", "partNumber", "website",
            "delete", "lifecycle", "tagging", "cors", "restore",
            "response-cache-contro", "response-content-disposition", "response-content-encoding",
            "response-content-language", "response-content-type", "response-expires"};


        /// <summary>
        /// Calculate the canonical string for a REST/HTTP request to KS3.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="resource"></param>
        /// <param name="request"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        public static string MakeKS3CanonicalString<T>(
            string method,
            string resource,
            IRequest<T> request, string expires) where T : KS3Request
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(method + "\n");

            // Add all interesting headers to a list, then sort them.  "Interesting"
            // is defined as Content-MD5, Content-Type, Date, and x-kss-
            IDictionary<string, string> headers = request.Headers;
            IDictionary<string, string> interestingHeaders = new SortedDictionary<string, string>();
            if (headers != null && headers.Count > 0)
            {
                foreach (var name in headers.Keys)
                {
                    var value = headers[name];
                    var lname = name.ToLower();

                    // Ignore any headers that are not particularly interesting.
                    if (lname.Equals(Headers.CONTENT_TYPE.ToLower()) || lname.Equals(Headers.CONTENT_MD5.ToLower()) || lname.Equals(Headers.DATE.ToLower()) ||
                        lname.StartsWith(Headers.KS3_PREFIX))
                    {
                        interestingHeaders.Add(lname, value);
                    }
                }
            }

            // Remove default date timestamp if "x-kss-date" is set.
            if (interestingHeaders.ContainsKey(Headers.KS3_ALTERNATE_DATE))
            {
                interestingHeaders[Headers.DATE.ToLower()] = "";
            }

            // Use the expires value as the timestamp if it is available. This trumps both the default
            // "date" timestamp, and the "x-kss-date" header.
            if (expires != null)
            {
                interestingHeaders[Headers.DATE.ToLower()] = expires;
            }
            // These headers require that we still put a new line in after them,
            // even if they don't exist.
            if (!interestingHeaders.ContainsKey(Headers.CONTENT_TYPE.ToLower()))
            {
                interestingHeaders.Add(Headers.CONTENT_TYPE.ToLower(), "");
            }
            if (!interestingHeaders.ContainsKey(Headers.CONTENT_MD5.ToLower()))
            {
                interestingHeaders.Add(Headers.CONTENT_MD5.ToLower(), "");
            }

            // Any parameters that are prefixed with "x-kss-" need to be included
            // in the headers section of the canonical string to sign
            foreach (var name in request.Parameters.Keys)
            {
                if (name.StartsWith(Headers.KS3_PREFIX))
                {
                    var value = request.Parameters[name];
                    interestingHeaders[name] = value;
                }
            }

            // Add all the interesting headers (i.e.: all that startwith x-kss- ;-))
            foreach (var name in interestingHeaders.Keys)
            {
                if (name.StartsWith(Headers.KS3_PREFIX))
                {
                    buf.Append(name + ":" + interestingHeaders[name]);
                }
                else
                {
                    buf.Append(interestingHeaders[name]);
                }
                buf.Append("\n");
            }

            // Add all the interesting parameters
            resource = resource.Replace("%5C", "/").Replace("//", "/%2F");
            resource = resource.EndsWith("%2F") ? resource.Substring(0, resource.Length - 3) : resource;
            buf.Append(resource);
            string[] parameterNames = request.Parameters.Keys.ToArray();
            Array.Sort(parameterNames);
            char separator = '?';
            foreach (string parameterName in parameterNames)
            {
                // Skip any parameters that aren't part of the canonical signed string
                if (!SIGNED_PARAMETERS.Contains(parameterName))
                {
                    continue;
                }
                buf.Append(separator);
                buf.Append(parameterName);
                string parameterValue = request.Parameters[parameterName];
                if (parameterValue != null)
                {
                    buf.Append("=" + parameterValue);
                }
                separator = '&';
            }

            return buf.ToString();
        }

        public static void PopulateObjectMetadata(HttpWebResponse response, ObjectMetadata metadata)
        {
            ISet<string> ignoredHeaders = new HashSet<string> {
                Headers.DATE,
                Headers.SERVER,
                Headers.REQUEST_ID,
                Headers.CONNECTION
            };

            foreach (string name in response.Headers.AllKeys)
            {
                if (name.StartsWith(Headers.KS3_USER_METADATA_PREFIX))
                {
                    string value = response.Headers[name];
                    string key = name.Substring(Headers.KS3_USER_METADATA_PREFIX.Length);
                    metadata.SetUserMetaData(key, value);
                }
                else if (ignoredHeaders.Contains(name))
                {
                    // ignore...
                }
                else if (name.Equals(Headers.LAST_MODIFIED))
                {
                    metadata.SetHeader(name, DateTime.Parse(response.Headers[name]));
                }
                else if (name.Equals(Headers.CONTENT_LENGTH))
                {
                    metadata.SetHeader(name, long.Parse(response.Headers[name]));
                }
                else if (name.Equals(Headers.ETAG))
                {
                    metadata.SetHeader(name, RemoveQuotes(response.Headers[name]));
                }
                else
                {
                    metadata.SetHeader(name, response.Headers[name]);
                }
            }
        }

        /// <summary>
        /// Removes any surrounding quotes from the specified string and returns a new string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string RemoveQuotes(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return string.Empty;
            }

            s = s.Trim();
            if (s.StartsWith("\""))
            {
                s = s.Substring(1);
            }
            if (s.EndsWith("\""))
            {
                s = s.Substring(0, s.Length - 1);
            }
            return s;
        }
    }
}
