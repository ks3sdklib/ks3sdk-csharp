using KS3.Http;
using System.Collections.Generic;

namespace KS3.Model
{
    public class CorsRule
    {
        /// <summary>
        /// Identifies an HTTP method that the domain/origin specified in the rule is allowed to execute
        /// </summary>
        public List<HttpMethod> AllowedMethods { get; set; } = new List<HttpMethod>();

        /// <summary>
        /// One or more response headers that you want customers to be able to access from their applications (for example, from a JavaScript XMLHttpRequest object).
        /// </summary>
        public List<string> AllowedOrigins { get; set; } = new List<string>();

        /// <summary>
        /// The time in seconds that your browser is to cache the preflight response for the specified resource. 
        /// </summary>
        public int MaxAgeSeconds { get; set; }

        /// <summary>
        /// One or more headers in the response that you want customers to be able to access from their applications (for example, from a JavaScript XMLHttpRequest object).
        /// </summary>
        public List<string> ExposedHeaders { get; set; } = new List<string>();

        /// <summary>
        ///Specifies which headers are allowed in a pre-flight OPTIONS request through the Access-Control-Request-Headers header. Each header name specified in the Access-Control-Request-Headers must have a corresponding entry in the rule. 
        /// </summary>
        public List<string> AllowedHeaders { get; set; } = new List<string>();



    }
}
