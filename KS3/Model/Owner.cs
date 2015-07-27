using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class Owner
    {
        private String displayName;
        private String id;

        public Owner() { }

        public Owner(String id, String displayName)
        {
            this.id = id;
            this.displayName = displayName;
        }

        public override String ToString()
        {
            return "KS3Owner [name=" + this.displayName + ", id=" + this.id + "]";
        }

        public String getId()
        {
            return this.id;
        }

        public void setId(String id)
        {
            this.id = id;
        }

        public String getDisplayName()
        {
            return this.displayName;
        }

        public void setDisplayName(String name)
        {
            this.displayName = name;
        }
    }
}
