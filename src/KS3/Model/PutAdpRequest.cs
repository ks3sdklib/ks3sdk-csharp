using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class PutAdpRequest:KS3Request
    {
        private String bucketName;

        public String BucketName
        {
            get { return bucketName; }
            set { bucketName = value; }
        }
        private String objectKey;

        public String ObjectKey
        {
            get { return objectKey; }
            set { objectKey = value; }
        }
        private IList<Adp> adps = new List<Adp>();

        public IList<Adp> Adps
        {
            get { return adps; }
            set { adps = value; }
        }
        /**
	 * 要进行的处理任务
	 */
        public void addAdp(Adp adp) {
            this.adps.Add(adp);
        }
        private String notifyURL;
        /**
	 * 数据处理任务完成后通知的url
	 */
        public String NotifyURL
        {
            get { return notifyURL; }
            set { notifyURL = value; }
        }

        private void validate(){
		    if(String.IsNullOrEmpty(bucketName))
			    throw new Exception("bucketname is not null");
		    if(String.IsNullOrEmpty(objectKey))
			    throw new Exception("objectKey is not null");
		    if(adps.Count==0){
			    throw new Exception("adps is not null");
		    }else{
			    foreach(Adp adp in adps){
				    if(String.IsNullOrEmpty(adp.Command)){
                        throw new Exception("adp's Command is not null");
				    }
			    }
		    }
		    if(String.IsNullOrEmpty(notifyURL))
                throw new Exception("notifyURL is not null");
	    }
        public String convertAdpsToString() {
            validate();
            StringBuilder fopStringBuffer = new StringBuilder();
		    foreach(Adp fop in adps){
			    fopStringBuffer.Append(fop.Command);
                fopStringBuffer.Append("|tag=saveas");
                if(!String.IsNullOrEmpty(fop.Bucket)){
                    fopStringBuffer.Append("&bucket="+fop.Bucket);
                }
                if(!String.IsNullOrEmpty(fop.Key)){
                    fopStringBuffer.Append("&object="+Convert.ToBase64String(Encoding.UTF8.GetBytes(fop.Key)));
                }
			    fopStringBuffer.Append(";");
		    }
		    return fopStringBuffer.ToString().TrimEnd(';');
            }
    }
}
