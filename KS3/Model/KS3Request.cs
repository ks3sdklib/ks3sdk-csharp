using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KS3.Auth;

namespace KS3.Model
{
    public abstract class KS3Request
    {
        /**
         * The optional credentials to use for this request - overrides the
         * default credentials set at the client level.
         */
        private KS3Credentials credentials;

        /**
	     * Sets the optional credentials to use for this request, overriding the
	     * default credentials set at the client level.
	     */
        public void setRequestCredentials(KS3Credentials credentials)
        {
            this.credentials = credentials;
        }

        /**
	     * Returns the optional credentials to use to sign this request, overriding
	     * the default credentials set at the client level.
	     */
        public KS3Credentials getRequestCredentials()
        {
            return this.credentials;
        }

        /**
         * Internal only method for accessing private, internal request parameters.
         * Not intended for direct use by callers.
         */
        public IDictionary<String, String> copyPrivateRequestParameters()
        {
            return new Dictionary<String, String>();
        }
    }
}
