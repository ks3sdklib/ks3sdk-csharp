using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class UploadPartRequest:KS3Request
    {

        public UploadPartRequest(String bucketname, String objectkey,
                String uploadId, int partNumber)
        {
            this.setBucketname(bucketname);
            this.setObjectkey(objectkey);
            this.setUploadId(uploadId);
            this.setPartNumber(partNumber);
        }
        private String bucketname;
	    private String objectkey;
        private String uploadId;
  
        private int partNumber;
        private ObjectMetadata metadata;
        private Stream inputStream;

        private IProgressListener progressListener;

        public void setBucketname(String bucketname) {
		this.bucketname = bucketname;
	    }
        public String getBucketname() {
            return this.bucketname;
        }
        public void setObjectkey(String objectkey)
        {
            this.objectkey = objectkey;
	    }
        public string getObjectkey() {
            return this.objectkey;
        }
        public String getUploadId()
        {
            return uploadId;
        }

        public void setUploadId(String uploadId)
        {
            this.uploadId = uploadId;
        }

        public int getPartNumber()
        {
            return partNumber;
        }

        public void setPartNumber(int partNumber)
        {
            this.partNumber = partNumber;
        }
        public void setProgressListener(IProgressListener progressListener)
        {
            this.progressListener = progressListener;
        }
        public IProgressListener getProgressListener()
        {
            return progressListener;
        }
        public ObjectMetadata getMetadata()
        {
            return this.metadata;
        }
        public void setMetadata(ObjectMetadata metadata)
        {
            this.metadata = metadata;
        }
        public Stream getInputStream()
        {
            return this.inputStream;
        }
        public void setInputStream(Stream inputStream)
        {
            this.inputStream = inputStream;
        }
    }
}
