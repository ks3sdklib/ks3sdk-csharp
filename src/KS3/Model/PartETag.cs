using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class PartETag
    {
        private int partNumber;
        private String eTag;

        public int getPartNumber()
        {
            return partNumber;
        }
        public void setPartNumber(int partNumber)
        {
            this.partNumber = partNumber;
        }
        public String geteTag()
        {
            return eTag;
        }
        public void seteTag(String eTag)
        {
            this.eTag = eTag;
        }
        public PartETag() { }
        public PartETag(int partNumber, String eTag)
        {
            this.partNumber = partNumber;
            this.eTag = eTag;
        }
        public override string ToString()
        {
            return "[partNum:" + this.partNumber + "][etag:" + this.eTag + "]";
        }

    }
}
