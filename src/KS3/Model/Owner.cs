using System;

namespace KS3.Model
{
    public class Owner
    {

        public string Id { get; set; }
        public string DisplayName { get; set; }

        public Owner() { }

        public Owner(string id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }

        public override String ToString()
        {
            return $"KS3Owner [name={DisplayName}, id={Id}]";
        }
    }
}
