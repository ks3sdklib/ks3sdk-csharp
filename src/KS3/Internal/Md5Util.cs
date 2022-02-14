using System.Security.Cryptography;
using System.Text;

namespace KS3.Internal
{
    public class Md5Util
    {
        public static byte[] Md5Digest(string message)
        {
            var result = Encoding.UTF8.GetBytes(message);
            MD5 md5 = MD5.Create();
            return md5.ComputeHash(result);
        }
    }
}
