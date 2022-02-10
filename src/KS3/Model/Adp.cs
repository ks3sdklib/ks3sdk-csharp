using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class Adp
    {
        /**
	 * 处理命令，详见KS3 API文档，数据处理
	 */
        private String command;

        public String Command
        {
            get { return command; }
            set { command = value; }
        }
        /**
	 * 数据处理成功后存储的bucket,如果不提供的话将会存在原数据的bucket下。
	 */
        private String bucket;

        public String Bucket
        {
            get { return bucket; }
            set { bucket = value; }
        }
        /**
	 * 数据处理成功后存储的key,如果不提供的话将会使用随机的key。
	 */
        private String key;

        public String Key
        {
            get { return key; }
            set { key = value; }
        }
    }
}
