using System.Collections.Generic;

namespace KS3.Model
{
    public class GetBucketLoggingResult
    {
        /// <summary>
        /// Container for logging information. This element and its children are present when logging is enabled, otherwise, this element and its children are absent.
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// Specifies the bucket whose logging status is being returned. This element specifies the bucket where server access logs will be delivered
        /// </summary>
        public string TargetBucket { get; set; }

        /// <summary>
        /// Specifies the prefix for the keys that the log files are being stored under.
        /// </summary>
        public string TargetPrefix { get; set; }

        /// <summary>
        /// Container for Grantee and Permission. (Postpone the opening)
        /// </summary>
        public ISet<Grant> TargetGrants { get; set; } = new HashSet<Grant>();
    }
}
