using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using KS3.Model;
using KS3.Internal;
using System.Xml;

namespace KS3.Transform
{
    public class ListBucketsUnmarshaller : Unmarshaller<IList<Bucket>, Stream>
    {
        public IList<Bucket> unmarshall(Stream inputStream)
        {
            Owner bucketsOwner = null;
            Bucket curBucket = null;
            StringBuilder currText = new StringBuilder();
            IList<Bucket> buckets = new List<Bucket>();
            
            XmlReader xr = XmlReader.Create(new BufferedStream(inputStream));
            while (xr.Read())
            {
                if (xr.NodeType.Equals(XmlNodeType.Element))
                {
                    if (xr.Name.Equals("Owner")) bucketsOwner = new Owner();
                    else if (xr.Name.Equals("Bucket")) curBucket = new Bucket();
                }
                else if (xr.NodeType.Equals(XmlNodeType.EndElement))
                {
                    if (xr.Name.Equals("DisplayName")) bucketsOwner.setDisplayName(currText.ToString());
                    else if (xr.Name.Equals("ID")) bucketsOwner.setId(currText.ToString());
                    else if (xr.Name.Equals("CreationDate")) curBucket.setCreationDate(DateTime.Parse(currText.ToString()));
                    else if (xr.Name.Equals("Name")) curBucket.setName(currText.ToString());
                    else if (xr.Name.Equals("Bucket"))
                    {
                        curBucket.setOwner(bucketsOwner);
                        buckets.Add(curBucket);
                    }
                    currText.Remove(0, currText.Length);
                }
                else if (xr.NodeType.Equals(XmlNodeType.Text))
                {
                    currText.Append(xr.Value);
                }

            }
            return buckets;
        }
    }
}
