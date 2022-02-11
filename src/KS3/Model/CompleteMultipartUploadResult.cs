namespace KS3.Model
{
    public class CompleteMultipartUploadResult
    {
        /// <summary>
        /// 新建对象的url
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 新建object存放的bucket
        /// </summary>
        public string Bucket { get; set; }

        /// <summary>
        /// 新建object存放的object key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 新建object的ETag
        /// </summary>
        public string ETag { get; set; }
    }
}
