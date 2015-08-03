using KS3.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class CorsRule
    {
        //Identifies an HTTP method that the domain/origin specified in the rule is allowed to execute
        private List<HttpMethod> allowedMethods = new List<HttpMethod>();

        public List<HttpMethod> AllowedMethods
        {
            get { return allowedMethods; }
            set { allowedMethods = value; }
        }
        //One or more response headers that you want customers to be able to access from their applications (for example, from a JavaScript XMLHttpRequest object).
        private List<String> allowedOrigins = new List<String>();

        public List<String> AllowedOrigins
        {
            get { return allowedOrigins; }
            set { allowedOrigins = value; }
        }
        //The time in seconds that your browser is to cache the preflight response for the specified resource. 
        private int maxAgeSeconds;

        public int MaxAgeSeconds
        {
            get { return maxAgeSeconds; }
            set { maxAgeSeconds = value; }
        }
        //One or more headers in the response that you want customers to be able to access from their applications (for example, from a JavaScript XMLHttpRequest object).
        private List<String> exposedHeaders = new List<String>();

        public List<String> ExposedHeaders
        {
            get { return exposedHeaders; }
            set { exposedHeaders = value; }
        }
        //Specifies which headers are allowed in a pre-flight OPTIONS request through the Access-Control-Request-Headers header. Each header name specified in the Access-Control-Request-Headers must have a corresponding entry in the rule. 
        private List<String> allowedHeaders=new List<String>();

        public List<String> AllowedHeaders
        {
            get { return allowedHeaders; }
            set { allowedHeaders = value; }
        }



    }
}
