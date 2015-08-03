using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class DeleteMultipleObjectsError
    {
        /**
	    *  object key
	    */
        private String key;

        public String Key
        {
            get { return key; }
            set { key = value; }
        }
        /**
         * Status code for the result of the failed delete，detail see <a href="http://ks3.ksyun.com/doc/api/index.html">http://ks3.ksyun.com/doc/api/index.html</a>
         */
        private String code;

        public String Code
        {
            get { return code; }
            set { code = value; }
        }
        /**
         * error message
         */
        private String message;

        public String Message
        {
            get { return message; }
            set { message = value; }
        }

    }
}
