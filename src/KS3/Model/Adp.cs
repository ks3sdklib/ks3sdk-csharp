namespace KS3.Model
{
    public class Adp
    {
        /// <summary>
        /// 处理命令，详见KS3 API文档，数据处理
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// 数据处理成功后存储的bucket,如果不提供的话将会存在原数据的bucket下。
        /// </summary>
        public string Bucket { get; set; }

        /// <summary>
        /// 数据处理成功后存储的key,如果不提供的话将会使用随机的key。
        /// </summary>
        public string Key { get; set; }
    }
}
