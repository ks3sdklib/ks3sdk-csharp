using System;

namespace KS3.Model
{
    public class Bucket
    {

        /// <summary>
        ///  The name of this KS3 bucket
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The details on the owner of this bucket
        /// </summary>
        public Owner Owner { get; set; }

        /// <summary>
        /// The date this bucket was created
        /// </summary>
        public DateTime? CreationDate { get; set; }

        public Bucket() { }

        public Bucket(string name)
        {
            Name = name;
        }

        public override String ToString()
        {
            return $"kS3Bucket [name={Name}, creationDate={CreationDate}, owner={Owner}]";
        }
    }
}
