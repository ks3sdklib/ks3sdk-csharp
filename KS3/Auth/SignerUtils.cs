using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Globalization;

using KS3.Internal;

namespace KS3.Auth
{
    public static class SignerUtils
    { 
        public static byte[] hmac_sha1(String secretKey, String strToSign)
        {
            byte[] key = Encoding.UTF8.GetBytes(secretKey);
            byte[] data = Encoding.UTF8.GetBytes(strToSign);
            HMACSHA1 hmac = new HMACSHA1(key);
            CryptoStream cs = new CryptoStream(Stream.Null, hmac, CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.Close();
            return hmac.Hash;
        }

        public static String base64(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        public static String getSignatrueDate(int timeOffset)
        {
            DateTime date = DateTime.UtcNow;
            if (timeOffset != 0) date = date.AddSeconds(-timeOffset);
            return getSignatrueDate(date);
        }

        public static String getSignatrueDate(DateTime date)
        {
            return date.ToString("ddd, dd MMM yyyy HH':'mm':'ss 'GMT'", CultureInfo.CreateSpecificCulture("en-US"));
        }
    }
}
