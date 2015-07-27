using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KS3.Internal;
using KS3.Model;

namespace KS3.Auth
{
    public class KS3Signer<T> : Signer<T> where T : KS3Request
    {
        /**
         * The HTTP verb (GET, PUT, HEAD, DELETE) the request to sign
         * is using.
         */
        private String httpVerb;

        /**
         * The canonical resource path portion of the S3 string to sign.
         * Examples: "/", "/<bucket name>/", or "/<bucket name>/<key>"
         */
        private String resourcePath;

        /**
         * Constructs a new KS3Signer to sign requests based on the
         * KS3 credentials, HTTP method and canonical KS3 resource path.
         */
        public KS3Signer(String httpVerb, String resourcePath)
        {
            this.httpVerb = httpVerb;
            this.resourcePath = resourcePath;
        }

        public void sign(Request<T> request, KS3Credentials credentials)
        {
            String date = SignerUtils.getSignatrueDate(request.getTimeOffset());
            request.setHeader(Headers.DATE, date);

            String canonicalString = RestUtils.makeKS3CanonicalString(httpVerb, resourcePath, request, null);

            String signature = SignerUtils.base64(SignerUtils.hmac_sha1(credentials.getKS3SecretKey(), canonicalString));
            request.setHeader("Authorization", "KSS " + credentials.getKS3AccessKeyId() + ":" + signature);
        }
        public String getSignature(KS3Credentials credentials, String expires)
        {
            var voidRequest = new DefaultRequest<NoneKS3Request>(new NoneKS3Request());
            String canonicalString = RestUtils.makeKS3CanonicalString(httpVerb, resourcePath, voidRequest, expires);
            String signature = SignerUtils.base64(SignerUtils.hmac_sha1(credentials.getKS3SecretKey(), canonicalString));
            return signature;
        }

    }
}
