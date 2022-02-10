using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    /**
     * Contains options to return a list of summary information about the objects in
     * the specified bucket. Depending on the request parameters, additional
     * information is returned, such as common prefixes if a delimiter was
     * specified.
     */
    public class ListObjectsRequest : KS3Request
    {
        /** The name of the KS3 bucket to list. */
        private String bucketName;

        /**
         * Optional parameter restricting the response to keys which begin with the
         * specified prefix. You can use prefixes to separate a bucket into
         * different sets of keys in a way similar to how a file system uses
         * folders.
         */
        private String prefix;

        /**
         * Optional parameter indicating where in the bucket to begin listing. The
         * list will only include keys that occur lexicographically after the
         * marker. This enables pagination; to get the next page of results use the
         * current value from ObjectListing.getNextMarker() as the marker
         * for the next request to list objects.
         */
        private String marker;

        /**
         * Optional parameter that causes keys that contain the same string between
         * the prefix and the first occurrence of the delimiter to be rolled up into
         * a single result element in the ObjectListing.getCommonPrefixes()
         * list. These rolled-up keys are not returned elsewhere in the response.
         * The most commonly used delimiter is "/", which simulates a hierarchical
         * organization similar to a file system directory structure.
         */
        private String delimiter;

        /**
         * Optional parameter indicating the maximum number of keys to include in
         * the response. KS3 might return fewer than this, but will not return more.
         * Even if maxKeys is not specified, KS3 will limit the number of results in
         * the response.
         */
        private int? maxKeys;

        /**
         * Constructs a new ListObjectsRequest object.
         */
        public ListObjectsRequest() { }

	    /**
	     * Constructs a new ListObjectsRequest object and initializes all
	     * required and optional object fields.
         */
        public ListObjectsRequest(String bucketName, String prefix, String marker, String delimiter, int? maxKeys)
        {
            this.bucketName = bucketName;
            this.prefix = prefix;
            this.marker = marker;
            this.delimiter = delimiter;
            this.maxKeys = maxKeys;
        }

	    /**
	     * Gets the name of the KS3 bucket whose objects are to be listed.
	     */
        public String getBucketName()
        {
            return this.bucketName;
        }

	    /**
	     * Sets the name of the KS3 bucket whose objects are to be listed.
	     */
        public void setBucketName(String bucketName)
        {
            this.bucketName = bucketName;
        }

	    /**
	     * Gets the optional prefix parameter and restricts the response to keys
	     * that begin with the specified prefix. Use prefixes to separate a bucket
	     * into different sets of keys, similar to how a file system organizes files
	     * into directories.
         */
        public String getPrefix()
        {
            return prefix;
        }

	    /**
	     * Sets the optional prefix parameter, restricting the response to keys that
	     * begin with the specified prefix.
         */
        public void setPrefix(String prefix)
        {
            this.prefix = prefix;
        }

	    /**
	     * Gets the optional marker parameter indicating where in the bucket to
	     * begin listing. The list will only include keys that occur
	     * lexicographically after the marker.
         */
        public String getMarker()
        {
            return marker;
        }

	    /**
	     * Sets the optional marker parameter indicating where in the bucket to
	     * begin listing. The list will only include keys that occur
	     * lexicographically after the marker.
	     */
        public void setMarker(String marker)
        {
            this.marker = marker;
        }

	    /**
	     * Gets the optional delimiter parameter that causes keys that contain the
	     * same string between the prefix and the first occurrence of the delimiter
	     * to be combined into a single result element in the
	     * ObjectListing.getCommonPrefixes() list. These combined keys are
	     * not returned elsewhere in the response. The most commonly used delimiter
	     * is "/", which simulates a hierarchical organization similar to a file
	     * system directory structure.
         */
        public String getDelimiter()
        {
            return this.delimiter;
        }

	    /**
	     * Sets the optional delimiter parameter that causes keys that contain the
	     * same string between the prefix and the first occurrence of the delimiter
	     * to be combined into a single result element in the
	     * ObjectListing.getCommonPrefixes() list.
         */
        public void setDelimiter(String delimiter)
        {
            this.delimiter = delimiter;
        }

	    /**
	     * Gets the optional <code>maxKeys</code> parameter indicating the maximum
	     * number of keys to include in the response. KS3 might return fewer keys
	     * than specified, but will never return more. Even if the optional
	     * parameter is not specified, KS3 will limit the number of results in the
	     * response.
         */
        public int? getMaxKeys()
        {
            return this.maxKeys;
        }

	    /**
	     * Sets the optional <code>maxKeys</code> parameter indicating the maximum
	     * number of keys to include in the response.
         */
        public void setMaxKeys(int? maxKeys)
        {
            this.maxKeys = maxKeys;
        }
    }
}
