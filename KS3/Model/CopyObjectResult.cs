using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class CopyObjectResult
    {
        private DateTime lastModified;

        public DateTime LastModified
        {
            get { return lastModified; }
            set { lastModified = value; }
        }
        private String eTag;

        public String ETag
        {
            get { return eTag; }
            set { eTag = value; }
        }
    }
}
