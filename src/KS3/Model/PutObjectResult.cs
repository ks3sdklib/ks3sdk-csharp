namespace KS3.Model
{
    /// <summary>
    /// Contains the data returned by KS3 from the <code>putObject</code> operation.
    /// </summary>
    public class PutObjectResult : KS3Request
    {
        /// <summary>
        /// The ETag value of the new object 
        /// </summary>
        public string ETag { get; set; }

        /// <summary>
        /// The content MD5
        /// </summary>
        public string ContentMD5 { get; set; }
    }
}
