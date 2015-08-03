using KS3.Http;
using KS3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace KS3.Internal
{
    public class HeadObjectResultHandler : HttpResponseHandler<HeadObjectResult> 
    {
        public HeadObjectResult handle(System.Net.HttpWebResponse response)
        {
            HeadObjectResult result = new HeadObjectResult();
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    convertObjectMetaData(result, response);
                    break;
                case HttpStatusCode.PartialContent:
                    convertObjectMetaData(result, response);
                    break;
                case HttpStatusCode.NotModified:
                    result.IfModified = false;
                    break;
                case HttpStatusCode.PreconditionFailed:
                    result.IfPreconditionSuccess = false;
                    break;
            }
            return result;
        }
        private void convertObjectMetaData(HeadObjectResult result,HttpWebResponse response) {
            ObjectMetadata metadata = new ObjectMetadata();
            RestUtils.populateObjectMetadata(response, metadata);
            result.ObjectMetadata = metadata;
        }
    }
}
