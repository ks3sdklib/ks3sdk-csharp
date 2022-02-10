/// <summary>
/// Provides access to the KS3 credentials used for accessing KS3 services: KS3 access key ID and secret access key. These credentials are used to securely sign requests to KS3 services.
/// </summary>
namespace KS3.Auth
{
    public interface IKS3Credentials
    {
        /// <summary>
        /// Returns the KS3 access key ID for this credentials object.
        /// </summary>
        /// <returns></returns>
        string GetKS3AccessKeyId();

        /// <summary>
        /// Returns the KS3 secret access key for this credentials object.
        /// </summary>
        /// <returns></returns>
        string GetKS3SecretKey();
    }
}
