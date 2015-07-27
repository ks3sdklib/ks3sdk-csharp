using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    /**
     * Contains the data returned by KS3 from the <code>putObject</code> operation.
     */
    public class PutObjectResult : KS3Request
    {
        /** The ETag value of the new object */
        private String eTag;

        /** The content MD5 */
        private String contentMD5;

        /**
         * Gets the ETag value for the newly created object.
         */
        public String getETag()
        {
            return eTag;
        }

        /**
         * Sets the ETag value for the new object that was created from the
         * associated <code>putObject</code> request.
         */
        public void setETag(String eTag)
        {
            this.eTag = eTag;
        }

        /**
         * Sets the content MD5.
         */
        public void setContentMD5(String contentMD5)
        {
            this.contentMD5 = contentMD5;
        }

        /**
	     * Returns the content MD5.
	     */
        public String getContentMD5()
        {
            return this.contentMD5;
        }
    }
}
