using System.Collections.Generic;

namespace KS3.Model
{
    public class AdpInfo
    {
        /// <summary>
        /// 数据处理命令
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// 是否处理成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 处理信息
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 数据Keys
        /// </summary>
        public IList<string> Keys { get; set; } = new List<string>();

    }
}
