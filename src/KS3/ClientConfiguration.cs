namespace KS3
{
    public class ClientConfiguration
    {
        //public static int DEFAULT_TIMEOUT = 15 * 60 * 1000;

        /// <summary>
        /// * The default time-out value in milliseconds for the GetResponse and  GetRequestStream methods of the HttpWebRequest.
        /// * To specify the amount of time to wait for the request to complete, use the Timeout property.
        /// </summary>
        public static int DEFAULT_TIMEOUT = int.MaxValue;

        //public static int DEFAULT_READ_WRITE_TIMEOUT = 15 * 60 * 1000;

        /// <summary>
        /// The default timeout for a connected socket. 
        /// </summary>
        public static int DEFAULT_READ_WRITE_TIMEOUT = int.MaxValue;

        /// <summary>
        /// The default max connection pool size.
        /// </summary>
        public static int DEFAULT_MAX_CONNECTIONS = 20;

        /// <summary>
        /// The default HTTP user agent header. 
        /// </summary>
        public static string DEFAULT_USER_AGENT = "KS3 User";

        /// <summary>
        /// The HTTP user agent header passed with all HTTP requests.
        /// </summary>
        public string UserAgent { get; set; } = DEFAULT_USER_AGENT;

        /// <summary>
        /// The protocol to use when connecting to KS3.
        /// </summary>
        public string Protocol { get; set; } = Http.Protocol.HTTP;

        /// <summary>
        /// Optionally specifies the proxy host to connect through.
        /// </summary>
        public string ProxyHost { get; set; }

        /// <summary>
        /// Optionally specifies the port on the proxy host to connect through.
        /// </summary>
        public int ProxyPort { get; set; } = -1;

        /// <summary>
        /// Optionally specifies the user name to use when connecting through a proxy.
        /// </summary>
        public string ProxyUsername { get; set; }

        /// <summary>
        /// Optionally specifies the password to use when connecting through a proxy.
        /// </summary>
        public string ProxyPassword { get; set; }

        /// <summary>
        /// The maximum number of open HTTP connections.
        /// </summary>
        public int MaxConnections { get; set; } = DEFAULT_MAX_CONNECTIONS;

        /// <summary>
        /// Gets or sets the time-out value in milliseconds for the GetResponse and  GetRequestStream methods of the HttpWebRequest.
        /// </summary>
        public int Timeout { get; set; } = DEFAULT_TIMEOUT;

        /// <summary>
        /// Gets or sets a time-out in milliseconds when writing to or reading from  a stream.
        /// </summary>
        public int ReadWriteTimeout { get; set; } = DEFAULT_READ_WRITE_TIMEOUT;


        public ClientConfiguration()
        {
        }

        public ClientConfiguration(ClientConfiguration other)
        {
            UserAgent = other.UserAgent;
            Protocol = other.Protocol;
            ProxyHost = other.ProxyHost;
            ProxyPort = other.ProxyPort;
            ProxyUsername = other.ProxyUsername;
            ProxyPassword = other.ProxyPassword;
            MaxConnections = other.MaxConnections;
            Timeout = other.Timeout;
            ReadWriteTimeout = other.ReadWriteTimeout;
        }

    }
}
