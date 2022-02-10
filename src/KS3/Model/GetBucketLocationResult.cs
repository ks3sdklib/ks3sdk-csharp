using KS3.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class GetBucketLocationResult
    {
        private Region region;

        public Region Region
        {
            get { return region; }
            set { region = value; }
        }
    }
}
