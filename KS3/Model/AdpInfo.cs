using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class AdpInfo
    {
        /**
	 * 数据处理命令
	 */
        private String command;

        public String Command
        {
            get { return command; }
            set { command = value; }
        }
        /**
         * 是否处理成功
         */
        private bool success;

        public bool Success
        {
            get { return success; }
            set { success = value; }
        }
        /**
         * 处理信息信息
         */
        private String desc;

        public String Desc
        {
            get { return desc; }
            set { desc = value; }
        }
        /**
         * 数据处理完成后新的数据的key
         */
        private IList<String> keys = new List<String>();

        public IList<String> Keys
        {
            get { return keys; }
            set { keys = value; }
        }
    }
}
