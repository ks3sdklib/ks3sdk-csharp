using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KS3.Http;

namespace KS3
{
    public class ClientConfiguration
    {
        /*
         * The default time-out value in milliseconds for the GetResponse and 
         * GetRequestStream methods of the HttpWebRequest.
         * 
         * To specify the amount of time to wait for the request to complete,
         * use the Timeout property.
         */
        //public static int DEFAULT_TIMEOUT = 15 * 60 * 1000;
        public static int DEFAULT_TIMEOUT = int.MaxValue;

        /** The default timeout for a connected socket. */
        //public static int DEFAULT_READ_WRITE_TIMEOUT = 15 * 60 * 1000;
        public static int DEFAULT_READ_WRITE_TIMEOUT = int.MaxValue;

        /** The default max connection pool size. */
        public static int DEFAULT_MAX_CONNECTIONS = 20;

        /** The default HTTP user agent header. */
        public static String DEFAULT_USER_AGENT = "KS3 User";

        /** The HTTP user agent header passed with all HTTP requests. */
        private String userAgent = DEFAULT_USER_AGENT;

        /**
         * The protocol to use when connecting to KS3.
         */
        private String protocol = Protocol.HTTP;

        /** Optionally specifies the proxy host to connect through. */
        private String proxyHost = null;

        /** Optionally specifies the port on the proxy host to connect through. */
        private int proxyPort = -1;

        /** Optionally specifies the user name to use when connecting through a proxy. */
        private String proxyUsername = null;

        /** Optionally specifies the password to use when connecting through a proxy. */
        private String proxyPassword = null;

        /** The maximum number of open HTTP connections. */
        private int maxConnections = DEFAULT_MAX_CONNECTIONS;

        /**
         * Gets or sets the time-out value in milliseconds for the GetResponse and 
         * GetRequestStream methods of the HttpWebRequest.
         */
        private int timeout = DEFAULT_TIMEOUT;

        /**
         * Gets or sets a time-out in milliseconds when writing to or reading from 
         * a stream.
         */
        private int readWriteTimeout = DEFAULT_READ_WRITE_TIMEOUT;


        public ClientConfiguration() { }

        public ClientConfiguration(ClientConfiguration other) {
            this.userAgent = other.userAgent;
            this.protocol = other.protocol;
            this.proxyHost = other.proxyHost;
            this.proxyPort = other.proxyPort;
            this.proxyUsername = other.proxyUsername;
            this.proxyPassword = other.proxyPassword;
            this.maxConnections = other.maxConnections;
            this.timeout = other.timeout;
            this.readWriteTimeout = other.readWriteTimeout;
        }

        /**
         * Returns the protocol (HTTP or HTTPS) to use when connecting to KS3.
         * The default configuration is to use HTTP.
         */
        public String getProtocol()
        {
            return this.protocol;
        }

        /**
         * Sets the protocol (i.e. HTTP or HTTPS) to use when connecting to KS3.
         */
        public void setProtocol(String protocol)
        {
            this.protocol = protocol;
        }

        /**
         * Returns the maximum number of allowed open HTTP connections.
         */
        public int getMaxConnections()
        {
            return this.maxConnections;
        }

        /**
         * Sets the maximum number of allowed open HTTP connections.
         */
        public void setMaxConnections(int maxConnections)
        { 
            this.maxConnections = maxConnections;
        }

        /**
         * Sets the HTTP user agent header to send with all requests.
         */
        public String getUserAgent()
        {
            return this.userAgent;
        }

        /**
         * Sets the HTTP user agent header used in requests and returns the updated
         * ClientConfiguration object.
         */
        public void setUserAgent(String userAgent)
        {
            this.userAgent = userAgent;
        }

        /**
         * Returns the optional proxy host the client will connect through.
         */
        public String getProxyHost()
        {
            return this.proxyHost;
        }

        /**
         * Sets the optional proxy host the client will connect through.
         */
        public void setProxyHost(String proxyHost)
        {
            this.proxyHost = proxyHost;
        }

        /**
         * Returns the optional proxy port the client will connect through.
         */
        public int getProxyPort()
        {
            return this.proxyPort;
        }

        /**
         * Sets the optional proxy port the client will connect through.
         */
        public void setProxyPort(int proxyPort)
        {
            this.proxyPort = proxyPort;
        }

        /**
         * Returns the optional proxy user name to use if connecting through a
         * proxy.
         */
        public String getProxyUsername()
        {
            return this.proxyUsername;
        }

        /**
         * Sets the optional proxy user name to use if connecting through a proxy.
         */
        public void setProxyUsername(String username)
        {
            this.proxyUsername = username;
        }

        /**
         * Returns the optional proxy password to use when connecting through a
         * proxy.
         */
        public String getProxyPassword()
        {
            return this.proxyPassword;
        }

        /**
         * Sets the optional proxy password to use when connecting through a proxy.
         */
        public void setProxyPassword(String proxyPassword)
        {
            this.proxyPassword = proxyPassword;
        }

        /** Returns the ReadWriteTimeout of the HttpWebRequest. */
        public int getReadWriteTimeout()
        {
            return this.readWriteTimeout;
        }

        /** Sets the ReadWriteTimeout of the HttpWebRequest. */
        public void setReadWriteTimeout(int readWriteTimeout)
        {
            this.readWriteTimeout = readWriteTimeout;
        }

        /** Returns the Timeout of the HttpWebRequest. */
        public int getTimeout()
        {
            return this.timeout;
        }

        /** Sets the Timeout of the HttpWebRequest. */
        public void setTimeout(int timeout)
        {
            this.timeout = timeout;
        }
    }
}
