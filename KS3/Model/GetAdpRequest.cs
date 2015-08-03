using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class GetAdpRequest:KS3Request
    {
        private String taskId;

        public String TaskId
        {
            get { return taskId; }
            set { taskId = value; }
        }
    }
}
