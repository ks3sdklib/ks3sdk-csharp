using KS3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace KS3.Transform
{
    public class ListBucketsUnmarshaller : IUnmarshaller<IList<Bucket>, Stream>
    {
        public IList<Bucket> Unmarshall(Stream inputStream)
        {
            Owner bucketsOwner = null;
            Bucket curBucket = null;
            StringBuilder currText = new StringBuilder();
            var buckets = new List<Bucket>();

            XmlReader xr = XmlReader.Create(new BufferedStream(inputStream));
            while (xr.Read())
            {
                if (xr.NodeType.Equals(XmlNodeType.Element))
                {
                    if (xr.Name.Equals("Owner"))
                    {
                        bucketsOwner = new Owner();
                    }
                    else if (xr.Name.Equals("Bucket"))
                    {
                        curBucket = new Bucket();
                    }
                }
                else if (xr.NodeType.Equals(XmlNodeType.EndElement))
                {
                    if (xr.Name.Equals("DisplayName"))
                    {
                        bucketsOwner.DisplayName = currText.ToString();
                    }
                    else if (xr.Name.Equals("ID"))
                    {
                        bucketsOwner.Id = currText.ToString();
                    }
                    else if (xr.Name.Equals("CreationDate"))
                    {
                        curBucket.CreationDate = DateTime.Parse(currText.ToString());
                    }
                    else if (xr.Name.Equals("Name"))
                    {
                        curBucket.Name = currText.ToString();
                    }
                    else if (xr.Name.Equals("Bucket"))
                    {
                        curBucket.Owner = bucketsOwner;
                        buckets.Add(curBucket);
                    }
                    currText.Clear();
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
