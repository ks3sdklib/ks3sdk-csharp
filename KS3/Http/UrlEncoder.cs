using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KS3.Internal;

namespace KS3.Http
{
    public static class UrlEncoder
    {
        private static String digits = "0123456789ABCDEF";

        public static String encode(String s, Encoding encoding)
        {
            if (s == null) return null;

            StringBuilder builder = new StringBuilder();
            int start = -1;
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];
                if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z')
                    || (ch >= '0' && ch <= '9' || " .-*_".IndexOf(ch) > -1))
                {
                    if (start >= 0)
                    {
                        convert(s.Substring(start, i - start), builder, encoding);
                        start = -1;
                    }
                    if (ch != ' ') builder.Append(ch);
                    else builder.Append('+');
                }
                else
                {
                    if (start < 0)
                        start = i;
                }
            }
            if (start >= 0)
                convert(s.Substring(start, s.Length - start), builder, encoding);

            return builder.ToString().Trim().Replace("+", "%20").Replace("*", "%2A").Replace("%2F", "/");
        }

        private static void convert(String s, StringBuilder builder, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(s);
            for (int j = 0; j < bytes.Length; j++)
            {
                builder.Append('%');
                builder.Append(digits[(bytes[j] & 0xf0) >> 4]);
                builder.Append(digits[bytes[j] & 0xf]);
            }
        }
    }
}
