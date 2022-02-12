namespace KS3.Model
{
    /// <summary>
    /// Represents a grantee identified by their canonical KS3 ID. The canonical KS3 ID can be thought of as an KS3-internal ID specific to a user.
    /// </summary>
    public class CanonicalGrantee : IGrantee
    {
        private string _id = string.Empty;
        private string _displayName = string.Empty;

        /// <summary>
        /// Constructs a new CanonicalGrantee object with the given canonical ID.
        /// </summary>
        /// <param name="id"></param>
        public CanonicalGrantee(string id)
        {
            _id = id;
        }

        public CanonicalGrantee(string id, string displayName)
        {
            _id = id;
            _displayName = displayName;
        }

        public string GetTypeIdentifier()
        {
            return "id";
        }

        /// <summary>
        /// Sets the unique identifier for this grantee.
        /// </summary>
        /// <param name="id"></param>
        public void SetIdentifier(string id)
        {
            _id = id;
        }

        /// <summary>
        /// Returns the unique identifier for this grantee.
        /// </summary>
        /// <returns></returns>
        public string GetIdentifier()
        {
            return _id;
        }

        /// <summary>
        /// Sets the display name for this grantee.
        /// </summary>
        /// <param name="displayName"></param>
        public void SetDisplayName(string displayName)
        {
            _displayName = displayName;
        }

        /// <summary>
        /// Returns the display name for this grantee.
        /// </summary>
        /// <returns></returns>
        public string GetDisplayName()
        {
            return _displayName;
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType().Equals(this.GetType()))
            {
                CanonicalGrantee other = (CanonicalGrantee)obj;
                return _id.Equals(other._id);
            }

            return false;
        }

        public override string ToString()
        {
            return $"CanonicalGrantee [id={_id}, displayName={_displayName}]";
        }
    }
}
