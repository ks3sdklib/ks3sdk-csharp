using System.Text;

namespace KS3.Http
{
    public static class UrlEncoder
    {
        private static readonly string digits = "0123456789ABCDEF";

        public static string Encode(string s, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return string.Empty;
            }

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
                        Convert(s.Substring(start, i - start), builder, encoding);
                        start = -1;
                    }
                    if (ch != ' ')
                    {
                        builder.Append(ch);
                    }
                    else
                    {
                        builder.Append('+');
                    }
                }
                else
                {
                    if (start < 0)
                    {
                        start = i;
                    }
                }
            }
            if (start >= 0)
            {
                Convert(s.Substring(start, s.Length - start), builder, encoding);
            }
            return builder.ToString().Trim().Replace("+", "%20").Replace("*", "%2A").Replace("%2F", "/");
        }

        private static void Convert(string s, StringBuilder builder, Encoding encoding)
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
