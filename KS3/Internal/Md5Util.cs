using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
 
namespace KS3.Internal
{
    public class Md5Util
    {
        public static byte[] md5Digest(String message) {
            byte[] result = Encoding.UTF8.GetBytes(message);
            MD5 md5 = MD5.Create();
            byte[] output = md5.ComputeHash(result);
            return output;
        }
    }
}
