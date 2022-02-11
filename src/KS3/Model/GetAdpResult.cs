using System.Collections.Generic;
using System.Xml.Linq;

namespace KS3.Model
{
    public class GetAdpResult
    {

        public XDocument Doc { get; set; }

        public string TaskId { get; set; }

        /// <summary>
        /// 	0,"task is create fail"、1,"task is create success"、2,"task is processing"、3,"task is process success"、4,"task is process fail"
        /// </summary>
        public string Processstatus { get; set; }

        public string Processdesc { get; set; }

        /// <summary>
        /// 0,"task is not notify"、1,"task is notify success"、2,"task is notify fail"
        /// </summary>
        public string Notifystatus { get; set; }

        public string Notifydesc { get; set; }

        /// <summary>
        /// 每条命令的具体处理结果
        /// </summary>
        public IList<AdpInfo> AdpInfos { get; set; } = new List<AdpInfo>();
    }
}
