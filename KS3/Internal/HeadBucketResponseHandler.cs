using KS3.Http;
using KS3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Internal
{
    public class HeadBucketResponseHandler : HttpResponseHandler<HeadBucketResult> 
    {
        public HeadBucketResult handle(System.Net.HttpWebResponse response)
        {
            HeadBucketResult result = new HeadBucketResult();
            result.StatueCode = response.StatusCode;
            return result;
        }
    }
}
