using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Security.Cryptography.X509Certificates;

using KS3.Auth;
using KS3.Model;
using KS3.Internal;
using KS3.Transform;
using KS3.KS3Exception;

namespace KS3.Http
{
    public class KS3HttpClient
    {
        /** Client configuration options, such as proxy settings, max retries, etc. */
        private ClientConfiguration config;

        private ErrorResponseHandler errorResponseHandler = new ErrorResponseHandler(new ErrorResponseUnmarshaller());

        public KS3HttpClient(ClientConfiguration clientConfiguration)
        {
            this.config = clientConfiguration;
            this.init();
        }

        private void init()
        {
            /* Setting for https proctol */
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

            /* Set max connections */
            int maxConnections = config.getMaxConnections();
            ServicePointManager.DefaultConnectionLimit = maxConnections;

            /* Set proxy if configured */
            String proxyHost = config.getProxyHost();
            int proxyPort = config.getProxyPort();
            if (proxyHost != null && proxyPort > 0)
            {
                WebProxy webProxy = new WebProxy(proxyHost, proxyPort);

                String proxyUsername = config.getProxyUsername();
                String proxyPassword = config.getProxyPassword();
                if (proxyUsername != null && proxyPassword != null)
                {
                    NetworkCredential credential = new NetworkCredential(proxyUsername, proxyPassword);
                    webProxy.Credentials = credential;
                }

                WebRequest.DefaultWebProxy = webProxy;
            }
            else
                WebRequest.DefaultWebProxy = null;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; // always accept
        }

        public X excute<X, Y>(Request<Y> request, HttpResponseHandler<X> responseHandler, KS3Signer<Y> ks3Signer) where Y : KS3Request
        {
            this.setUserAgent(request);

            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;
            X result=default(X);
            for (int i = 0; i < Constants.RETRY_TIMES;i++ )
            {
                try
                {
                    // Sign the request if a signer was provided
                    if (ks3Signer != null && request.getOriginalRequest().getRequestCredentials() != null)
                        ks3Signer.sign(request, request.getOriginalRequest().getRequestCredentials());

                    httpRequest = HttpRequestFactory.createHttpRequest(request, this.config);
                    httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                    result = responseHandler.handle(httpResponse);
                    break;
                }
                catch (WebException we)
                {
                    HttpWebResponse errorResponse = (HttpWebResponse)we.Response;
                    ServiceException serviceException = null;
                    try
                    {
                        serviceException = errorResponseHandler.handle(errorResponse);
                    }
                    catch
                    {
                        throw we;
                    }
                    throw serviceException;
                }
                catch (IOException ex) {
                    if (i == Constants.RETRY_TIMES-1)
                    {
                        throw ex;
                    }
                }
                finally
                {
                    if (httpResponse != null)
                        httpResponse.Close();
                }
            }
            return result;
            
        }

        /**
         * Sets a User-Agent for the specified request, taking into account
         * any custom data.
         */   
        private void setUserAgent<T>(Request<T> request) where T : KS3Request
        {
            String userAgent = config.getUserAgent();
            if (userAgent != null) request.setHeader(Headers.USER_AGENT, userAgent);
        }
    }
}
