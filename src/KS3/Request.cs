using KS3.Http;
using KS3.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace KS3
{
    public interface IRequest<T>
    {
        /// <summary>
        /// Set header
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void SetHeader(string name, string value);

        /// <summary>
        /// Set parameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void SetParameter(string name, string value);

        /// <summary>
        /// Map of the parameters being sent as part of this request
        /// </summary>
        IDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// Map of the headers included in this request
        /// </summary>
        IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// The resource path being requested
        /// </summary>
        string ResourcePath { get; set; }

        /// <summary>
        /// The service endpoint to which this request should be sent
        /// </summary>
        Uri Endpoint { get; set; }

        /// <summary>
        ///  The original, user facing request object which this internal request object is representing
        /// </summary>
        KS3Request OriginalRequest { get; set; }

        /// <summary>
        /// The HTTP method to use when sending this request.
        /// </summary>
        HttpMethod HttpMethod { get; set; }

        /// <summary>
        /// An optional stream from which to read the request payload.
        /// </summary>
        Stream Content { get; set; }

        /// <summary>
        /// An optional time offset to account for clock skew
        /// </summary>
        int TimeOffset { get; set; }
    }
}
