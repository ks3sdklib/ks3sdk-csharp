using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class HeadObjectRequest:KS3Request
    {
        private String bucketName;

        public String BucketName
        {
            get { return bucketName; }
            set { bucketName = value; }
        }
        private String objectKey;

        public String ObjectKey
        {
            get { return objectKey; }
            set { objectKey = value; }
        }
        /**
        * Downloads the specified range bytes of an object. For more information about the HTTP Range header, go to http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.35.
        */
        private String range = null;

        public String Range
        {
            get { return range; }
            set { range = value; }
        }
        /**
         * Return the object only if its entity tag (ETag) is the same as the one specified; otherwise, return a 412 (precondition failed).
         */
        private IList<String> matchingETagConstraints = new List<String>();

        public IList<String> MatchingETagConstraints
        {
            get { return matchingETagConstraints; }
            set { matchingETagConstraints = value; }
        }
        /**
         * Return the object only if its entity tag (ETag) is different from the one specified; otherwise, return a 304 (not modified).
         */
        private IList<String> nonmatchingEtagConstraints = new List<String>();

        public IList<String> NonmatchingEtagConstraints
        {
            get { return nonmatchingEtagConstraints; }
            set { nonmatchingEtagConstraints = value; }
        }
        /**
         * Return the object only if it has not been modified since the specified time, otherwise return a 412 (precondition failed).
         */
        private DateTime unmodifiedSinceConstraint = DateTime.MinValue;

        public DateTime UnmodifiedSinceConstraint
        {
            get { return unmodifiedSinceConstraint; }
            set { unmodifiedSinceConstraint = value; }
        }
        /**
         * Return the object only if it has been modified since the specified time, otherwise return a 304 (not modified).
         */
        private DateTime modifiedSinceConstraint = DateTime.MinValue;

        public DateTime ModifiedSinceConstraint
        {
            get { return modifiedSinceConstraint; }
            set { modifiedSinceConstraint = value; }
        }
        /**
         * modify the return response's headers
         */
        private ResponseHeaderOverrides overrides = new ResponseHeaderOverrides();

        public ResponseHeaderOverrides Overrides
        {
            get { return overrides; }
            set { overrides = value; }
        }

        public HeadObjectRequest() { }
        public HeadObjectRequest(String bucketName,String objectKey) {
            this.bucketName = bucketName;
            this.objectKey = objectKey;
        }
        public void validate() {
            if (String.IsNullOrEmpty(bucketName))
                throw new Exception("bucketname is not null");
            if (String.IsNullOrEmpty(objectKey))
                throw new Exception("objectKey is not null");
            if (!String.IsNullOrEmpty(range))
            {
                if (!range.StartsWith("bytes="))
                    throw new Exception("Range rule must be bytes=x-y,y>=x");
            }
        }
    }
}
