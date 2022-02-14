using System;
using System.Collections.Generic;

namespace KS3.KS3Exception
{
    public class ServiceException : Exception
    {
        public string RequestId { get; set; }
        public string ErrorCode { get; set; }
        public int StatusCode { get; set; }

        public ServiceException() : base() { }

        /// <summary>
        /// Constructs a new KS3ClientException with the specified message.
        /// </summary>
        /// <param name="message"></param>
        public ServiceException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructs a new KS3ClientException with the specified message and exception indicating the root cause.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cause"></param>
        public ServiceException(String message, Exception cause) : base(message, cause) { }

        /// <summary>
        /// Returns a string summary of the details of this exception including the HTTP status code, KS3 request ID, KS3 error code and error message.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var l = new List<string>()
            {
                "ServiceException:",
                $"Status Code: {StatusCode}" ,
                $"Request ID: {RequestId}",
                $"Error Code: {ErrorCode}" ,
                $"Error Message: {Message}"
            };

            return string.Join("\n", l);
        }
    }
}
