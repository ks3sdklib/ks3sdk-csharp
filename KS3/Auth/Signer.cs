using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Auth
{
    public interface Signer<T>
    {
        void sign(Request<T> request, KS3Credentials credentials);
    }
}
