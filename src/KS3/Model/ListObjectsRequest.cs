namespace KS3.Model
{
    /// <summary>
    /// Contains options to return a list of summary information about the objects in the specified bucket. Depending on the request parameters, additionalinformation is returned, such as common prefixes if a delimiter wasspecified.
    /// </summary>
    public class ListObjectsRequest : KS3Request
    {
        /// <summary>
        /// The name of the KS3 bucket to list.
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// Optional parameter restricting the response to keys which begin with the specified prefix.You can use prefixes to separate a bucket into different sets of keys in a way similar to how a file system uses folders.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        ///   * Optional parameter indicating where in the bucket to begin listing. The list will only include keys that occur lexicographically after the marker.This enables pagination; to get the next page of results use the current value from ObjectListing.getNextMarker() as the marker for the next request to list objects.
        /// </summary>
        public string Marker { get; set; }

        /// <summary>
        ///  Optional parameter that causes keys that contain the same string between the prefix and the first occurrence of the delimiter to be rolled up into a single result element in the ObjectListing.getCommonPrefixes() list.These rolled-up keys are not returned elsewhere in the response. The most commonly used delimiter is "/", which simulates a hierarchical organization similar to a file system directory structure.
        /// </summary>
        public string Delimiter { get; set; }

        /// <summary>
        /// Optional parameter indicating the maximum number of keys to include in the response.KS3 might return fewer than this, but will not return more. Even if maxKeys is not specified, KS3 will limit the number of results in the response.
        /// </summary>
        public int? MaxKeys { get; set; }


        /// <summary>
        /// Constructs a new ListObjectsRequest object.
        /// </summary>
        public ListObjectsRequest() { }

        /// <summary>
        /// Constructs a new ListObjectsRequest object and initializes all required and optional object fields.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="prefix"></param>
        /// <param name="marker"></param>
        /// <param name="delimiter"></param>
        /// <param name="maxKeys"></param>
        public ListObjectsRequest(string bucketName, string prefix, string marker, string delimiter, int? maxKeys)
        {
            BucketName = bucketName;
            Prefix = prefix;
            Marker = marker;
            Delimiter = delimiter;
            MaxKeys = maxKeys;
        }
    
    }
}
