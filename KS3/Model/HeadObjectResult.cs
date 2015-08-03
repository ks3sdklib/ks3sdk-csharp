using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class HeadObjectResult
    {
        private ObjectMetadata objectMetadata = new ObjectMetadata();

        public ObjectMetadata ObjectMetadata
        {
            get { return objectMetadata; }
            set { objectMetadata = value; }
        }

        private bool ifModified = true;

        public bool IfModified
        {
            get { return ifModified; }
            set { ifModified = value; }
        }

        private bool ifPreconditionSuccess = true;

        public bool IfPreconditionSuccess
        {
            get { return ifPreconditionSuccess; }
            set { ifPreconditionSuccess = value; }
        }
    }
}
