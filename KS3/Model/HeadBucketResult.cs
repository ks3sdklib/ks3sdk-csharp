using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace KS3.Model
{
    public class HeadBucketResult
    {
        /// <summary>
        /// The operation returns a 200 OK if the bucket exists and you have permission to access it. 
        /// Otherwise, the operation might return responses such as 404 Not Found and 403 Forbidden.  
        /// </summary>
        private HttpStatusCode statueCode;

        public HttpStatusCode StatueCode
        {
            get { return statueCode; }
            set { statueCode = value; }
        }

    }
}
