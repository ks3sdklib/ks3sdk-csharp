using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace KS3.Http
{
    public interface HttpResponseHandler<T>
    {
        /**
         * Accepts an HTTP response object, and returns an object of type T.
         * Individual implementations may choose to handle the response however they
         * need to, and return any type that they need to.
         */
        T handle(HttpWebResponse response);
    }
}
