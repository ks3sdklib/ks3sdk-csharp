using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace KS3.Extensions
{
    public static class XmlExtensions
    {
        public static string GetSingleValueOrDefault(this IEnumerable<XElement> elements, string name, string defaultValue = "")
        {
            var element = elements.Where(w => w.Name.LocalName == name).FirstOrDefault();
            if (element != null)
            {
                return element.Value;
            }
            return defaultValue;
        }

    }
}
