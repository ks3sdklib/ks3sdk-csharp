using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class GetBucketLoggingResult
    {
        //Container for logging information. This element and its children are present when logging is enabled, otherwise, this element and its children are absent.
        private Boolean enable = false;

        public Boolean Enable
        {
            get { return enable; }
            set { enable = value; }
        }
        //Specifies the bucket whose logging status is being returned. This element specifies the bucket where server access logs will be delivered
        private String targetBucket;

        public String TargetBucket
        {
            get { return targetBucket; }
            set { targetBucket = value; }
        }
        //Specifies the prefix for the keys that the log files are being stored under.
        private String targetPrefix;

        public String TargetPrefix
        {
            get { return targetPrefix; }
            set { targetPrefix = value; }
        }
        //Container for Grantee and Permission. (Postpone the opening)
        private HashSet<Grant> targetGrants = new HashSet<Grant>();

        public HashSet<Grant> TargetGrants
        {
            get { return targetGrants; }
            set { targetGrants = value; }
        }
    }
}
