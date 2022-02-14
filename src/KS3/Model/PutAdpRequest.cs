using KS3.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace KS3.Model
{
    public class PutAdpRequest : KS3Request
    {
        public string BucketName { get; set; }
        public string ObjectKey { get; set; }
        public string NotifyURL { get; set; }
        public IList<Adp> Adps { get; set; } = new List<Adp>();

        public void AddAdp(Adp adp)
        {
            Adps.Add(adp);
        }


        private void Validate()
        {
            if (BucketName.IsNullOrWhiteSpace())
            {
                throw new Exception("bucketname is not null");
            }

            if (ObjectKey.IsNullOrWhiteSpace())
            {
                throw new Exception("objectKey is not null");
            }

            if (Adps.Count == 0)
            {
                throw new Exception("adps is not null");
            }
            else
            {

                foreach (var adp in Adps)
                {
                    if (adp.Command.IsNullOrWhiteSpace())
                    {
                        throw new Exception("adp's Command is not null");
                    }
                }
            }

            if (NotifyURL.IsNullOrWhiteSpace())
            {
                throw new Exception("notifyURL is not null");
            }
        }

        public string ConvertAdpsToString()
        {
            Validate();
            StringBuilder fopStringBuffer = new StringBuilder();
            foreach (Adp fop in Adps)
            {
                fopStringBuffer.Append(fop.Command);
                fopStringBuffer.Append("|tag=saveas");
                if (!fop.Bucket.IsNullOrWhiteSpace())
                {
                    fopStringBuffer.Append("&bucket=" + fop.Bucket);
                }
                if (!fop.Key.IsNullOrWhiteSpace())
                {
                    fopStringBuffer.Append("&object=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(fop.Key)));
                }
                fopStringBuffer.Append(";");
            }
            return fopStringBuffer.ToString().TrimEnd(';');
        }
    }
}
