using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KS3.Transform
{
    public interface Unmarshaller<X, Y>
    {
        X unmarshall(Y input);
    }
}
