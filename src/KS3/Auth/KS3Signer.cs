using KS3.Internal;
using KS3.Model;
using System;

namespace KS3.Auth
{
    public class KS3Signer<T> : ISigner<T> where T : KS3Request
    {
        /// <summary>
        /// The HTTP verb (GET, PUT, HEAD, DELETE) the request to sign is using.
        /// </summary>
        private readonly string _httpVerb;

        /// <summary>
        /// The canonical resource path portion of the S3 string to sign. Examples: "/", "/<bucket name>/", or "/<bucket name>/<key>"
        /// </summary>
        private readonly string _resourcePath;

        /// <summary>
        ///  Constructs a new KS3Signer to sign requests based on the KS3 credentials, HTTP method and canonical KS3 resource path.
        /// </summary>
        /// <param name="httpVerb"></param>
        /// <param name="resourcePath"></param>
        public KS3Signer(string httpVerb, string resourcePath)
        {
            _httpVerb = httpVerb;
            _resourcePath = resourcePath;
        }

        public void Sign(IRequest<T> request, IKS3Credentials credentials)
        {
            var date = SignerUtils.GetSignatrueDate(request.TimeOffset);
            request.SetHeader(Headers.DATE, date);

            var canonicalString = RestUtils.MakeKS3CanonicalString(_httpVerb, _resourcePath, request, null);

            var signature = SignerUtils.Base64(SignerUtils.Hmacsha1(credentials.GetKS3SecretKey(), canonicalString));
            request.SetHeader("Authorization", "KSS " + credentials.GetKS3AccessKeyId() + ":" + signature);
        }
        public string GetSignature(IKS3Credentials credentials, String expires)
        {
            var voidRequest = new DefaultRequest<NoneKS3Request>(new NoneKS3Request());
            var canonicalString = RestUtils.MakeKS3CanonicalString(_httpVerb, _resourcePath, voidRequest, expires);
            var signature = SignerUtils.Base64(SignerUtils.Hmacsha1(credentials.GetKS3SecretKey(), canonicalString));
            return signature;
        }

    }
}
