using System.Collections.Generic;

namespace KS3.Model
{
    public class BucketCorsConfigurationResult
    {
        public IList<CorsRule> Rules { get; set; } = new List<CorsRule>();

        public void AddCorsRule(CorsRule corsRule)
        {
            Rules.Add(corsRule);
        }

    }
}
