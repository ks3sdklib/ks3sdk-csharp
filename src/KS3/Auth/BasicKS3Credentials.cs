namespace KS3.Auth
{
    /// <summary>
    /// Basic implementation of the KS3Credentials interface that allows callers to pass in the KS3 access key and secret access in the constructor.
    /// </summary>
    public class BasicKS3Credentials : IKS3Credentials
    {
        private readonly string _accessKey;
        private readonly string _secretKey;

        /// <summary>
        /// Constructs a new BasicKS3Credentials object, with the specified KS3 access key and KS3 secret key.
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        public BasicKS3Credentials(string accessKey, string secretKey)
        {
            _accessKey = accessKey;
            _secretKey = secretKey;
        }

        public string GetKS3AccessKeyId()
        {
            return _accessKey;
        }

        public string GetKS3SecretKey()
        {
            return _secretKey;
        }
    }
}
