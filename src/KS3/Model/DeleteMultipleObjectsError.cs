namespace KS3.Model
{
    public class DeleteMultipleObjectsError
    {
        /// <summary>
        /// Object key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Status code for the result of the failed delete，detail see <a href="http://ks3.ksyun.com/doc/api/index.html">http://ks3.ksyun.com/doc/api/index.html</a>
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }
    
    }
}
