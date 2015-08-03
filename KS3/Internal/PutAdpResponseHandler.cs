using KS3.Http;
using KS3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace KS3.Internal
{
    public class PutAdpResponseHandler:HttpResponseHandler<PutAdpResult> 
    {
        public PutAdpResult handle(System.Net.HttpWebResponse response)
        {
            PutAdpResult result = new PutAdpResult();
            result.Status = response.StatusCode;
            if (HttpStatusCode.OK.Equals(response.StatusCode))
            {
                result.TaskId = response.Headers.Get(Headers.TaskId);
            }
            return result;
        }
    }
}
