using System;

namespace KS3.Model
{
    public class CopyObjectResult
    {
        public DateTime LastModified { get; set; }
        public string ETag { get; set; }
    }
}
