using KS3.Model;
using System.Collections.Generic;
using System.Text;

namespace KS3.Transform
{
    public static class AclXmlFactory
    {
        private static readonly string _xmlns = "http://www.w3.org/2001/XMLSchema-instance";

        public static string ConvertToXmlString(AccessControlList acl)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<AccessControlPolicy>");
            builder.Append(ConvertOwner(acl.Owner));
            builder.Append(ConvertGrants(acl.Grants));
            builder.Append("</AccessControlPolicy>");

            return builder.ToString();
        }

        private static string ConvertOwner(Owner owner)
        {
            if (owner == null)
            {
                return string.Empty;
            }
            return "<Owner><DisplayName>" + owner.DisplayName + "</DisplayName><ID>" + owner.Id + "</ID></Owner>";
        }

        private static string ConvertGrants(ISet<Grant> grants)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<AccessControlList>");
            foreach (Grant grant in grants)
            {
                builder.Append(ConvertGrant(grant));
            }
            builder.Append("</AccessControlList>");

            return builder.ToString();
        }

        private static string ConvertGrant(Grant grant)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Grant>");
            builder.Append(ConvertGrantee(grant.Grantee));
            builder.Append(ConvertPermission(grant.Permission));
            builder.Append("</Grant>");

            return builder.ToString();
        }

        private static string ConvertGrantee(IGrantee grantee)
        {
            if (grantee.GetType().Equals(typeof(CanonicalGrantee)))
            {
                return ConvertCanonicalGrantee((CanonicalGrantee)grantee);
            }
            else if (grantee.GetType().Equals(typeof(GroupGrantee)))
            {
                return ConvertGroupGrantee((GroupGrantee)grantee);
            }
            return null;
        }

        private static string ConvertCanonicalGrantee(CanonicalGrantee grantee)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Grantee xmlns:xsi=\"" + _xmlns + "\" xsi:type=\"CanonicalUser\">");
            builder.Append("<DisplayName>" + grantee.GetDisplayName() + "</DisplayName>");
            builder.Append("<ID>" + grantee.GetIdentifier() + "</ID>");
            builder.Append("</Grantee>");

            return builder.ToString();
        }

        private static string ConvertGroupGrantee(GroupGrantee grantee)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Grantee xmlns:xsi=\"" + _xmlns + "\" xsi:type=\"Group\">");
            builder.Append("<URI>" + grantee.GetIdentifier() + "</URI>");
            builder.Append("</Grantee>");

            return builder.ToString();
        }

        private static string ConvertPermission(string permission)
        {
            return "<Permission>" + permission + "</Permission>";
        }
    }
}
