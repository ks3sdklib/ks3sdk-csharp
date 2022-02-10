using KS3.Model;
using System.IO;
using System.Text;
using System.Xml;

namespace KS3.Transform
{
    public class AccessControlListUnmarshaller : IUnmarshaller<AccessControlList, Stream>
    {
        public AccessControlList Unmarshall(Stream inputStream)
        {
            AccessControlList acl = new AccessControlList();
            string ownerId = null;
            string ownerDisplayName = null;
            IGrantee grantee = null;
            string granteeType = null;
            string userId = null;
            string userDisplayName = null;
            string groupUri = null;
            string permission = null;
            bool insideGrant = false;
            StringBuilder currText = new StringBuilder();

            XmlReader xr = XmlReader.Create(new BufferedStream(inputStream));
            while (xr.Read())
            {
                if (xr.NodeType.Equals(XmlNodeType.Element))
                {
                    if (xr.Name.Equals("Grant"))
                        insideGrant = true;
                    else if (xr.Name.Equals("Grantee"))
                        granteeType = xr.GetAttribute("xsi:type");
                }
                else if (xr.NodeType.Equals(XmlNodeType.EndElement))
                {
                    if (xr.Name.Equals("DisplayName"))
                    {
                        if (!insideGrant)
                            ownerId = currText.ToString();
                        else
                            userId = currText.ToString();
                    }
                    else if (xr.Name.Equals("ID"))
                    {
                        if (!insideGrant)
                        {
                            ownerDisplayName = currText.ToString();
                        }
                        else
                        {
                            userDisplayName = currText.ToString();
                        }
                    }
                    else if (xr.Name.Equals("URI"))
                    {
                        groupUri = currText.ToString();
                    }
                    else if (xr.Name.Equals("Owner"))
                    {
                        Owner owner = new Owner(ownerId, ownerDisplayName);
                        acl.Owner = owner;
                    }
                    else if (xr.Name.Equals("Grantee"))
                    {
                        if (granteeType.Equals("CanonicalUser"))
                        {
                            grantee = new CanonicalGrantee(userId, userDisplayName);
                        }
                        else if (granteeType.Equals("Group"))
                        {
                            grantee = new GroupGrantee(groupUri);
                        }
                    }
                    else if (xr.Name.Equals("Permission"))
                    {
                        permission = currText.ToString();
                    }
                    else if (xr.Name.Equals("Grant"))
                    {
                        acl.GrantPermission(grantee, permission);
                        insideGrant = false;
                    }

                    currText.Clear();
                }
                else if (xr.NodeType.Equals(XmlNodeType.Text))
                {
                    currText.Append(xr.Value);
                }
            } // end while

            return acl;
        } // end of unmarshall
    }
}
