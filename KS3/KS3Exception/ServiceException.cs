using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.KS3Exception
{
    public class ServiceException : Exception
    {
        private String requestId;

        /**
         * The KS3 error code represented by this exception (ex:
         * InvalidParameterValue).
         */
        private String errorCode;

        /** The HTTP status code that was returned with this error */
        private int statusCode;

        public ServiceException() : base() { }

        /**
         * Constructs a new KS3ClientException with the specified message.
         */
        public ServiceException(String message) : base(message) { }

        /**
         * Constructs a new KS3ClientException with the specified message and
         * exception indicating the root cause.
         */
        public ServiceException(String message, Exception cause) : base(message, cause) { }

        /**
         * Sets the KS3 requestId for this exception.
         */
        public void setRequestId(String requestId)
        {
            this.requestId = requestId;
        }

        /**
         * Returns the KS3 request ID that uniquely identifies the service request
         * the caller made.
         */
        public String getRequestId()
        {
            return this.requestId;
        }

        /**
         * Sets the KS3 error code represented by this exception.
         */
        public void setErrorCode(String errorCode)
        {
            this.errorCode = errorCode;
        }

        /**
         * Sets the KS3 error code represented by this exception.
         */
        public String getErrorCode()
        {
            return this.errorCode;
        }

        /**
         * Sets the HTTP status code that was returned with this service exception.
         */
        public void setStatusCode(int statusCode)
        {
            this.statusCode = statusCode;
        }

        /**
         * Returns the HTTP status code that was returned with this service
         * exception.
         */
        public int getStatusCode()
        {
            return statusCode;
        }

        /**
         * Returns a string summary of the details of this exception including the
         * HTTP status code, KS3 request ID, KS3 error code and error message.
         */
        public override string ToString()
        {
            
            return String.Join("\n", new String[] {
                   "ServiceException:",
                   "Status Code: " + this.statusCode,
                   "Request ID: " + this.requestId,
                   "Error Code: " + this.errorCode,
                   "Error Message: " + this.Message});
        }
    }
}
