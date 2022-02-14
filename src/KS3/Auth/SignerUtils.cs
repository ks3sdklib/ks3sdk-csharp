using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace KS3.Auth
{
    public static class SignerUtils
    { 
        public static byte[] Hmacsha1(string secretKey, string strToSign)
        {
            byte[] key = Encoding.UTF8.GetBytes(secretKey);
            byte[] data = Encoding.UTF8.GetBytes(strToSign);
            HMACSHA1 hmac = new HMACSHA1(key);
            CryptoStream cs = new CryptoStream(Stream.Null, hmac, CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.Close();
            return hmac.Hash;
        }

        public static string Base64(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        public static string GetSignatrueDate(int timeOffset)
        {
            DateTime date = DateTime.UtcNow;
            if (timeOffset != 0)
            {
                date = date.AddSeconds(-timeOffset);
            }
            return GetSignatrueDate(date);
        }

        public static string GetSignatrueDate(DateTime date)
        {
            return date.ToString("ddd, dd MMM yyyy HH':'mm':'ss 'GMT'", CultureInfo.CreateSpecificCulture("en-US"));
        }
    }
}
