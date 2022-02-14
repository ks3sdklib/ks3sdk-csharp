using KS3.Auth;
using System.Collections.Generic;

namespace KS3.Model
{
    public abstract class KS3Request
    {
        /// <summary>
        /// The optional credentials to use for this request - overrides the default credentials set at the client level.
        /// </summary>
        public IKS3Credentials Credentials { get; set; }

        /// <summary>
        /// Internal only method for accessing private, internal request parameters. Not intended for direct use by callers.
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> CopyPrivateRequestParameters()
        {
            return new Dictionary<string, string>();
        }
    }
}
