using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace KS3.Model
{
    public class GetAdpResult
    {
        private XDocument doc;

        public XDocument Doc
        {
            get { return doc; }
            set { doc = value; }
        }
        /**
	 * taskid
	 */
        private String taskId;

        public String TaskId
        {
            get { return taskId; }
            set { taskId = value; }
        }
        /**
         * 	0,"task is create fail"、1,"task is create success"、2,"task is processing"、3,"task is process success"、4,"task is process fail"
         */
        private String processstatus;

        public String Processstatus
        {
            get { return processstatus; }
            set { processstatus = value; }
        }
        private String processdesc;

        public String Processdesc
        {
            get { return processdesc; }
            set { processdesc = value; }
        }
        /**
         * 0,"task is not notify"、1,"task is notify success"、2,"task is notify fail"
         */
        private String notifystatus;

        public String Notifystatus
        {
            get { return notifystatus; }
            set { notifystatus = value; }
        }
        private String notifydesc;

        public String Notifydesc
        {
            get { return notifydesc; }
            set { notifydesc = value; }
        }
        /**
         * 每条命令的具体处理结果
         */
        private IList<AdpInfo> adpInfos = new List<AdpInfo>();

        public IList<AdpInfo> AdpInfos
        {
            get { return adpInfos; }
            set { adpInfos = value; }
        }
    }
}
