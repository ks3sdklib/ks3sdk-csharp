using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class BucketCorsConfigurationResult
    {
        private IList<CorsRule> rules = new List<CorsRule>();

        public IList<CorsRule> Rules
        {
            get { return rules; }
            set { rules = value; }
        }
        public void addCorsRule(CorsRule corsRule) {
            this.rules.Add(corsRule);
        }

    }
}
