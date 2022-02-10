using System;

namespace KS3.Model
{
    /// <summary>
    ///  Represents a grantee (entity) that can be assigned access permissions in an AccessControlList. All grantees have an ID of some kind, though the format of the ID can differ depending on the kind of grantee.
    /// </summary>
    public interface IGrantee
    {
        /// <summary>
        /// Returns the identifier for the type of this grant, to be used when specifying grants in the header of a request.
        /// </summary>
        /// <returns></returns>
        string GetTypeIdentifier();

        /// <summary>
        /// Sets the identifier for this grantee. The meaning of the identifier is specific to each implementation of the Grantee.
        /// </summary>
        /// <param name="id"></param>
        void SetIdentifier(string id);

        /// <summary>
        /// Gets the identifier for this grantee. The meaning of the grantee identifier is specific to each implementation of the Grantee.
        /// </summary>
        /// <returns></returns>
        string GetIdentifier();
    }
}
