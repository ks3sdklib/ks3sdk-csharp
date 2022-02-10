using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class ListMultipartUploadsResult
    {
        private String bucketname;
        private String objectkey;
        private String uploadId;
        private bool isTruncated;
        private IList<Part> parts=new List<Part>();
        public ListMultipartUploadsResult() { }

        public void setBucketname(String bucketname)
        {
            this.bucketname = bucketname;
        }
        public String getBucketname()
        {
            return this.bucketname;
        }
        public void setObjectkey(String objectkey)
        {
            this.objectkey = objectkey;
        }
        public string getObjectkey()
        {
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
        public void setIsTruncated(bool isTruncated) {
            this.isTruncated = isTruncated;
        }
        public bool getIsTruncated() {
            return this.isTruncated;
        }
        public IList<Part> getParts(){
            return this.parts;
        }
        public void setParts(IList<Part> parts) {
            this.parts = parts;
        }
    }
    public class Part {
        private int partNumber;

        public int PartNumber
        {
            get { return partNumber; }
            set { partNumber = value; }
        }
        private string eTag;

        public string ETag
        {
            get { return eTag; }
            set { eTag = value; }
        }
        private DateTime lastModified;

        public DateTime LastModified
        {
            get { return lastModified; }
            set { lastModified = value; }
        }
        private int size;

        public int Size
        {
            get { return size; }
            set { size = value; }
        }

    }
}
