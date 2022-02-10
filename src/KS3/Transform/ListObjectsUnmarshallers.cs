using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

using KS3.Model;

namespace KS3.Transform
{
    class ListObjectsUnmarshallers : IUnmarshaller<ObjectListing, Stream>
    {
        public ObjectListing Unmarshall(Stream inputStream)
        {
            ObjectSummary currObject = null;
            Owner currOwner = null;
            StringBuilder currText = new StringBuilder();
            Boolean insideCommonPrefixes = false;

            ObjectListing objectListing = new ObjectListing();

            String bucketName = null;
            String lastKey = null;
            Boolean truncated = false;
            String nextMarker = null;
            
            XmlReader xr = XmlReader.Create(new BufferedStream(inputStream));
            while (xr.Read())
            {
                if (xr.NodeType.Equals(XmlNodeType.Element))
                {
                    if (xr.Name.Equals("Contents"))
                        currObject = new ObjectSummary();
                    else if (xr.Name.Equals("CommonPrefixes"))
                        insideCommonPrefixes = true;
                    else if (xr.Name.Equals("Owner"))
                        currOwner = new Owner();
                    
                }
                else if (xr.NodeType.Equals(XmlNodeType.EndElement))
                {
                    if (xr.Name.Equals("Name"))
                        bucketName = currText.ToString();
                    else if (xr.Name.Equals("Delimiter"))
                    {
                        String s = currText.ToString();
                        if (s.Length > 0)
                            objectListing.setDelimiter(s);
                    }
                    else if (xr.Name.Equals("MaxKeys"))
                    {
                        String s = currText.ToString();
                        if (s.Length > 0)
                            objectListing.setMaxKeys(int.Parse(currText.ToString()));
                    }
                    else if (xr.Name.Equals("Prefix"))
                    {
                        if (insideCommonPrefixes)
                            objectListing.getCommonPrefixes().Add(currText.ToString());
                        else
                        {
                            String s = currText.ToString();
                            if (s.Length > 0)
                                objectListing.setPrefix(s);
                        }
                    }
                    else if (xr.Name.Equals("Marker"))
                    {
                        String s = currText.ToString();
                        if (s.Length > 0)
                            objectListing.setMarker(s);
                    }
                    else if (xr.Name.Equals("NextMarker"))
                        nextMarker = currText.ToString();
                    else if (xr.Name.Equals("IsTruncated"))
                    {
                        truncated = Boolean.Parse(currText.ToString());
                        objectListing.setTruncated(truncated);
                    }
                    else if (xr.Name.Equals("Contents"))
                    {
                        currObject.setBucketName(bucketName);
                        objectListing.getObjectSummaries().Add(currObject);
                    }
                    else if (xr.Name.Equals("Owner"))
                        currObject.setOwner(currOwner);
                    else if (xr.Name.Equals("DisplayName"))
                        currOwner.setDisplayName(currText.ToString());
                    else if (xr.Name.Equals("ID"))
                        currOwner.setId(currText.ToString());
                    else if (xr.Name.Equals("LastModified"))
                        currObject.setLastModified(DateTime.Parse(currText.ToString()));
                    else if (xr.Name.Equals("ETag"))
                        currObject.setETag(currText.ToString());
                    else if (xr.Name.Equals("CommonPrefixes"))
                        insideCommonPrefixes = false;
                    else if (xr.Name.Equals("Key"))
                    {
                        lastKey = currText.ToString();
                        currObject.setKey(lastKey);
                    }
                    else if (xr.Name.Equals("Size"))
                        currObject.setSize(long.Parse(currText.ToString()));

                    currText.Clear();
                }
                else if (xr.NodeType.Equals(XmlNodeType.Text))
                {
                    currText.Append(xr.Value);
                }
            }
            
            objectListing.setBucketName(bucketName);

            if (truncated)
            {
                if (nextMarker == null && lastKey != null)
                    nextMarker = lastKey;
                objectListing.setNextMarker(nextMarker);
            }
            
            return objectListing;
        } // end of unmarshall
    }
}
