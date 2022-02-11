using KS3.Extensions;
using KS3.Model;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace KS3.Transform
{
    class ListObjectsUnmarshallers : IUnmarshaller<ObjectListing, Stream>
    {
        public ObjectListing Unmarshall(Stream inputStream)
        {
            ObjectSummary currObject = null;
            Owner currOwner = null;
            StringBuilder currText = new StringBuilder();
            bool insideCommonPrefixes = false;

            ObjectListing objectListing = new ObjectListing();

            string bucketName = null;
            string lastKey = null;
            bool truncated = false;
            string nextMarker = null;

            XmlReader xr = XmlReader.Create(new BufferedStream(inputStream));
            while (xr.Read())
            {
                if (xr.NodeType.Equals(XmlNodeType.Element))
                {
                    if (xr.Name.Equals("Contents"))
                    {
                        currObject = new ObjectSummary();
                    }
                    else if (xr.Name.Equals("CommonPrefixes"))
                    {
                        insideCommonPrefixes = true;
                    }
                    else if (xr.Name.Equals("Owner"))
                    {
                        currOwner = new Owner();
                    }
                }
                else if (xr.NodeType.Equals(XmlNodeType.EndElement))
                {
                    if (xr.Name.Equals("Name"))
                    {
                        bucketName = currText.ToString();
                    }
                    else if (xr.Name.Equals("Delimiter"))
                    {
                        string s = currText.ToString();
                        if (s.Length > 0)
                        {
                            objectListing.Delimiter = s;
                        }
                    }
                    else if (xr.Name.Equals("MaxKeys"))
                    {
                        string s = currText.ToString();
                        if (s.Length > 0)
                        {
                            objectListing.MaxKeys = int.Parse(currText.ToString());
                        }
                    }
                    else if (xr.Name.Equals("Prefix"))
                    {
                        if (insideCommonPrefixes)
                        {
                            objectListing.CommonPrefixes.Add(currText.ToString());
                        }
                        else
                        {
                            string s = currText.ToString();
                            if (s.Length > 0)
                            {
                                objectListing.Prefix = s;
                            }
                        }
                    }
                    else if (xr.Name.Equals("Marker"))
                    {
                        string s = currText.ToString();
                        if (s.Length > 0)
                        {
                            objectListing.Marker = s;
                        }
                    }
                    else if (xr.Name.Equals("NextMarker"))
                    {
                        nextMarker = currText.ToString();
                    }
                    else if (xr.Name.Equals("IsTruncated"))
                    {
                        truncated = bool.Parse(currText.ToString());
                        objectListing.Truncated = truncated;
                    }
                    else if (xr.Name.Equals("Contents"))
                    {
                        currObject.BucketName = bucketName;
                        objectListing.ObjectSummaries.Add(currObject);
                    }
                    else if (xr.Name.Equals("Owner"))
                    {
                        currObject.Owner = currOwner;
                    }
                    else if (xr.Name.Equals("DisplayName"))
                    {
                        currOwner.DisplayName = currText.ToString();
                    }
                    else if (xr.Name.Equals("ID"))
                    {
                        currOwner.Id = currText.ToString();
                    }
                    else if (xr.Name.Equals("LastModified"))
                    {
                        currObject.LastModified = DateTime.Parse(currText.ToString());
                    }
                    else if (xr.Name.Equals("ETag"))
                    {
                        currObject.ETag = currText.ToString();
                    }
                    else if (xr.Name.Equals("CommonPrefixes"))
                    {
                        insideCommonPrefixes = false;
                    }
                    else if (xr.Name.Equals("Key"))
                    {
                        lastKey = currText.ToString();
                        currObject.Key = lastKey;
                    }
                    else if (xr.Name.Equals("Size"))
                    {
                        currObject.Size = long.Parse(currText.ToString());
                    }
                    currText.Clear();
                }
                else if (xr.NodeType.Equals(XmlNodeType.Text))
                {
                    currText.Append(xr.Value);
                }
            }

            objectListing.BucketName = bucketName;

            if (truncated)
            {
                if (nextMarker.IsNullOrWhiteSpace() && !lastKey.IsNullOrWhiteSpace())
                {
                    nextMarker = lastKey;
                }

                objectListing.NextMarker = nextMarker;
            }

            return objectListing;
        }
    }
}
