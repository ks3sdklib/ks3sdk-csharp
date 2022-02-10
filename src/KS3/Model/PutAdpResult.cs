using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace KS3.Model
{
    public class PutAdpResult
    {
        private HttpStatusCode status;

        public HttpStatusCode Status
        {
            get { return status; }
            set { status = value; }
        }
        private String taskId;

        public String TaskId
        {
            get { return taskId; }
            set { taskId = value; }
        }
    }
}
