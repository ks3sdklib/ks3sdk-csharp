using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Auth
{
    /**
     * Basic implementation of the KS3Credentials interface that allows callers to
     * pass in the KS3 access key and secret access in the constructor.
     */
    public class BasicKS3Credentials : KS3Credentials
    {
        private String accessKey;
        private String secretKey;
    
        /**
         * Constructs a new BasicKS3Credentials object, with the specified KS3
         * access key and KS3 secret key.
         */
        public BasicKS3Credentials(String accessKey, String secretKey)
        {
            this.accessKey = accessKey;
            this.secretKey = secretKey;
        }

        public String getKS3AccessKeyId()
        {
            return this.accessKey;
        }

        public String getKS3SecretKey()
        {
            return this.secretKey;
        }
    }
}
