using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KS3.Model
{
    /**
     * Provides options for downloading an KS3 object.
     */
    public class GetObjectRequest : KS3Request
    {
        /** The name of the bucket containing the object to retrieve */
        private String bucketName;

        /** The key under which the desired object is stored */
        private String key;

        /** Where the content will be stored */
        private FileInfo destinationFile;

        /** Optional member indicating the byte range of data to retrieve */
        private long[] range;

        /**
         * Optional list of ETag values that constrain this request to only be
         * executed if the object's ETag matches one of the specified ETag values.
         */
        private IList<String> matchingETagConstraints = new List<String>();

        /**
         * Optional list of ETag values that constrain this request to only be
         * executed if the object's ETag does not match any of the specified ETag
         * constraint values.
         */
        private IList<String> nonmatchingETagContraints = new List<String>();

        /**
         * Optional field that constrains this request to only be executed if the
         * object has not been modified since the specified date.
         */
        private DateTime? unmodifiedSinceConstraint;

        /**
         * Optional field that constrains this request to only be executed if the
         * object has been modified since the specified date.
         */
        private DateTime? modifiedSinceConstraint;

        /**
         * The optional progress listener for receiving updates about object download
         * status.
         */
        private ProgressListener progressListener;

        /**
         * Constructs a new GetObjectRequest with all the required parameters.
         */
        public GetObjectRequest(String bucketName, String key)
        {
            this.bucketName = bucketName;
            this.key = key;
        }

        /**
         * Constructs a new GetObjectRequest with all the required parameters.
         */
        public GetObjectRequest(String bucketName, String key, FileInfo destinationFile)
        {
            this.bucketName = bucketName;
            this.key = key;
            this.destinationFile = destinationFile;
        }

	    /**
	     * Gets the name of the bucket containing the object to be downloaded.
	     */
        public String getBucketName()
        {
            return this.bucketName;
        }

	    /**
	     * Sets the name of the bucket containing the object to be downloaded.
	     */
        public void setBucketName(String bucketName)
        {
            this.bucketName = bucketName;
        }

	    /**
	     * Gets the key under which the object to be downloaded is stored.
	     */
        public String getKey()
        {
            return key;
        }

	    /**
	     * Sets the key under which the object to be downloaded is stored.
	     */
        public void setKey(String key)
        {
            this.key = key;
        }

	    /**
	     * Gets where the content will be stored.
	     */
        public FileInfo getDestinationFile()
        {
            return this.destinationFile;
        }

        /**
	     * Sets where the content will be stored.
	     */
        public void setDestinationFile(FileInfo destinationFile)
        {
            this.destinationFile = destinationFile;
        }

	    /**
	     * <p>
	     * Gets the optional inclusive byte range within the desired object that
	     * will be downloaded by this request.
	     * </p>
	     * <p>
	     * The range is returned as a two element array, containing the start and
	     * end index of the byte range. If no byte range has been specified, the
	     * entire object is downloaded and this method returns <code>null</code>.
	     * </p>
         */
        public long[] getRange()
        {
            return range;
        }

	    /**
	     * <p>
	     * Sets the optional inclusive byte range within the desired object that
	     * will be downloaded by this request.
	     * </p
	     * <p>
	     * If no byte range is specified, this request downloads the entire object
	     * from KS3.
	     * </p>
         */
        public void setRange(long start, long end)
        {
            this.range = new long[] { start, end };
        }

	    /**
	     * Gets the optional list of ETag constraints that, when present,
	     * <b>must</b> include a match for the object's current ETag in order for
	     * this request to be executed. Only one ETag in the list needs to match for
	     * this request to be executed by KS3.
	     */
        public IList<String> getMatchingETagConstraints()
        {
            return this.matchingETagConstraints;
        }

	    /**
	     * Sets the optional list of ETag constraints that when present <b>must</b>
	     * include a match for the object's current ETag in order for this request
	     * to be executed. If none of the specified ETags match the object's current
	     * ETag, this request will not be executed. Only one ETag in the list needs
	     * to match for the request to be executed by KS3.
	     */
        public void setMatchingETagConstraints(List<String> eTagList)
        {
            this.matchingETagConstraints = eTagList;
        }

	    /**
	     * Gets the optional list of ETag constraints that when present, <b>must</b>
	     * not include a match for the object's current ETag in order for this
	     * request to be executed. If any entry in the non-matching ETag constraint
	     * list matches the object's current ETag, this request <b>will not</b> be
	     * executed by KS3.
	     */
        public IList<String> getNonmatchingETagConstraints()
        {
            return this.nonmatchingETagContraints;
        }

	    /**
	     * Sets the optional list of ETag constraints that when present <b>must</b>
	     * not include a match for the object's current ETag in order for this
	     * request to be executed. If any entry in the non-matching ETag constraint
	     * list matches the object's current ETag, this request <b>will not</b> be
	     * executed by KS3.
	     */
        public void setNonmatchingETagConstraints(List<String> eTagList)
        {
            this.nonmatchingETagContraints = eTagList;
        }

	    /**
	     * Gets the optional unmodified constraint that restricts this request to
	     * executing only if the object has <b>not</b> been modified after the
	     * specified date.
	     */
        public DateTime? getUnmodifiedSinceConstraint()
        {
            return this.unmodifiedSinceConstraint;
        }

	    /**
	     * Sets the optional unmodified constraint that restricts this request to
	     * executing only if the object has <b>not</b> been modified after the
	     * specified date.
	     */
        public void setUnmodifiedSinceConstraint(DateTime date)
        {
            this.unmodifiedSinceConstraint = date;
        }

	    /**
	     * Gets the optional modified constraint that restricts this request to
	     * executing only if the object <b>has</b> been modified after the specified
	     * date.
	     */
        public DateTime? getModifiedSinceConstraint()
        {
            return this.modifiedSinceConstraint;
        }

	    /**
	     * Sets the optional modified constraint that restricts this request to
	     * executing only if the object <b>has</b> been modified after the specified
	     * date.
         */
        public void setModifiedSinceConstraint(DateTime date)
        {
            this.modifiedSinceConstraint = date;
        }

	    /**
	     * Sets the optional progress listener for receiving updates about object
	     * download status.
	     */
        public void setProgressListener(ProgressListener progressListener)
        {
            this.progressListener = progressListener;
        }

	    /**
	     * Returns the optional progress listener for receiving updates about object
	     * download status.
	     */
        public ProgressListener getProgressListener()
        {
            return this.progressListener;
        }
    }
}
