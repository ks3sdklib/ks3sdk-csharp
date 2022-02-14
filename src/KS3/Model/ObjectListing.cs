using KS3.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace KS3.Model
{
    /// <summary>
    /// Contains the results of listing the objects in an KS3 bucket.
    /// </summary>
    public class ObjectListing
    {

        /// <summary>
        /// A list of summary information describing the objects stored in the bucket
        /// </summary>
        public IList<ObjectSummary> ObjectSummaries { get; set; } = new List<ObjectSummary>();

        /// <summary>
        ///  A list of the common prefixes included in this object listing - common prefixes will only be populated for requests that specified a delimiter
        /// </summary>
        public IList<string> CommonPrefixes { get; set; } = new List<string>();

        /// <summary>
        /// The name of the KS3 bucket containing the listed objects
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// The marker to use in order to request the next page of results - only populated if the isTruncated member indicates that this object listing is truncated
        /// </summary>
        public string NextMarker { get; set; }

        /// <summary>
        /// Indicates if this is a complete listing, or if the caller needs to make additional requests to KS3 to see the full object listing for an KS3 bucket
        /// </summary>
        public bool Truncated { get; set; }

        /// <summary>
        /// The prefix parameter originally specified by the caller when this object listing was returned
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// The marker parameter originally specified by the caller when this object listing was returned
        /// </summary>
        public string Marker { get; set; }

        /// <summary>
        /// The maxKeys parameter originally specified by the caller when this object listing was returned
        /// </summary>
        public int? MaxKeys { get; set; }

        /// <summary>
        /// The delimiter parameter originally specified by the caller when this object listing was returned
        /// </summary>
        public string Delimiter { get; set; }

        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"ObjectListing [bucketName={BucketName}");
            if (!Delimiter.IsNullOrWhiteSpace())
            {
                builder.Append($", delimiter={Delimiter}");
            }
            if (MaxKeys.HasValue)
            {
                builder.Append($", maxKeys={MaxKeys}");
            }
            if (!Prefix.IsNullOrWhiteSpace())
            {
                builder.Append($", prefix={Prefix}");
            }
            if (!Marker.IsNullOrWhiteSpace())
            {
                builder.Append($", marker={Marker}");
            }
            if (!NextMarker.IsNullOrWhiteSpace())
            {
                builder.Append($", nextMarker={NextMarker}");
            }
            builder.Append($", isTruncated={Truncated}]");

            foreach (var objectSummary in ObjectSummaries)
            {
                builder.Append($"\nObject:\n{objectSummary.ToString()}");
            }

            foreach (var s in CommonPrefixes)
            {
                builder.Append($"\nCommonPrefix:\n{s}");
            }

            return builder.ToString();
        }
    }
}
