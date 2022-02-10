using System.Text;

namespace KS3.Internal
{
    public static class Constants
    {
        /// <summary>
        ///  Default hostname for the KS3 service endpoint 
        /// </summary>
        public static string KS3_HOSTNAME = "ks3-cn-beijing.ksyun.com";

        /** Default encoding used for text data */
        public static Encoding DEFAULT_ENCODING = Encoding.UTF8;

        public static int KB = 1024;
        public static int MB = 1024 * KB;
        public static int GB = 1024 * MB;

        /// <summary>
        /// The default size of the buffer when uploading data from a stream. A buffer of this size will be created and filled with the first bytes from a stream being uploaded so that any transmit errors that occur in that section of the data can be automatically retried without the caller's intervention.
        /// </summary>
        public static int DEFAULT_STREAM_BUFFER_SIZE = 128 * KB;

        /// <summary>
        /// cors max rules limit
        /// </summary>
        public static int CorsMaxRules = 100;

        /// <summary>
        /// http connect IOException retry times
        /// </summary>
        public const int RETRY_TIMES = 3;
    }
}
