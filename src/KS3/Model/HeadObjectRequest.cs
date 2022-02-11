using System;
using System.Collections.Generic;

namespace KS3.Model
{
    public class HeadObjectRequest : KS3Request
    {

        public string BucketName { get; set; }
        public string ObjectKey { get; set; }

        /// <summary>
        /// Downloads the specified range bytes of an object. For more information about the HTTP Range header, go to http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.35.
        /// </summary>
        public string Range { get; set; }

        /// <summary>
        /// Return the object only if its entity tag (ETag) is the same as the one specified; otherwise, return a 412 (precondition failed).
        /// </summary>
        public IList<string> MatchingETagConstraints { get; set; } = new List<string>();


        /// <summary>
        /// Return the object only if its entity tag (ETag) is different from the one specified; otherwise, return a 304 (not modified).
        /// </summary>
        public IList<string> NonmatchingEtagConstraints { get; set; } = new List<string>();


        /// <summary>
        /// Return the object only if it has not been modified since the specified time, otherwise return a 412 (precondition failed).
        /// </summary>
        public DateTime UnmodifiedSinceConstraint { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Return the object only if it has been modified since the specified time, otherwise return a 304 (not modified).
        /// </summary>
        public DateTime ModifiedSinceConstraint { get; set; }

        /// <summary>
        /// modify the return response's headers
        /// </summary>
        public ResponseHeaderOverrides Overrides { get; set; } = new ResponseHeaderOverrides();

        public HeadObjectRequest()
        {
        }
        
        public HeadObjectRequest(string bucketName, string objectKey)
        {
            BucketName = bucketName;
            ObjectKey = objectKey;
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(BucketName))
            {
                throw new Exception("bucketname is not null");
            }
            if (string.IsNullOrWhiteSpace(ObjectKey))
            {
                throw new Exception("objectKey is not null");
            }
            if (!string.IsNullOrWhiteSpace(Range))
            {
                if (!Range.StartsWith("bytes="))
                {
                    throw new Exception("Range rule must be bytes=x-y,y>=x");
                }
            }
        }
    }
}
