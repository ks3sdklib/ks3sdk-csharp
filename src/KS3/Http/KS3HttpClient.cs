using KS3.Auth;
using KS3.Internal;
using KS3.KS3Exception;
using KS3.Model;
using KS3.Transform;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using KS3.Extensions;

namespace KS3.Http
{
    public class KS3HttpClient
    {
        /// <summary>
        /// Client configuration options, such as proxy settings, max retries, etc.
        /// </summary>
        private readonly ClientConfiguration _config;

        private readonly ErrorResponseHandler _errorResponseHandler;

        public KS3HttpClient(ClientConfiguration clientConfiguration)
        {
            _config = clientConfiguration;
            _errorResponseHandler = new ErrorResponseHandler(new ErrorResponseUnmarshaller());
            Init();
        }

        private void Init()
        {
            //Setting for https proctol
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

            //Set max connections
            ServicePointManager.DefaultConnectionLimit = _config.MaxConnections;

            //Set proxy if configured
            //String proxyHost = _config.ProxyHost;
            //int proxyPort = _config.ProxyPort;

            if (!_config.ProxyHost.IsNullOrWhiteSpace() && _config.ProxyPort > 0)
            {
                var webProxy = new WebProxy(_config.ProxyHost, _config.ProxyPort);

                if (!_config.ProxyUsername.IsNullOrWhiteSpace() && !_config.ProxyPassword.IsNullOrWhiteSpace())
                {
                    var credential = new NetworkCredential(_config.ProxyUsername, _config.ProxyPassword);
                    webProxy.Credentials = credential;
                }
                WebRequest.DefaultWebProxy = webProxy;
            }
            else
            {
                WebRequest.DefaultWebProxy = null;
            }
        }

        private static bool CheckValidationResult(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors errors)
        {
            return true; // always accept
        }

        public X Excute<X, Y>(
            IRequest<Y> request,
            IHttpResponseHandler<X> responseHandler,
            KS3Signer<Y> ks3Signer) where Y : KS3Request
        {
            //Set user agent
            SetUserAgent(request);

            HttpWebResponse httpResponse = null;
            X result = default;
            for (int i = 0; i < Constants.RETRY_TIMES; i++)
            {
                try
                {
                    // Sign the request if a signer was provided
                    if (ks3Signer != null && request.OriginalRequest.Credentials != null)
                    {
                        ks3Signer.Sign(request, request.OriginalRequest.Credentials);
                    }

                    request.ResourcePath = request.ResourcePath.Replace("%5C", "/").Replace("//", "/%2F");

                    if (request.ResourcePath.EndsWith("%2F"))
                    {
                        request.ResourcePath = (request.ResourcePath.Substring(0, request.ResourcePath.Length - 3));
                    }

                    HttpWebRequest httpRequest = HttpRequestFactory.CreateHttpRequest(request, this._config);
                    httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                    result = responseHandler.Handle(httpResponse);
                    break;
                }
                catch (WebException we)
                {
                    HttpWebResponse errorResponse = (HttpWebResponse)we.Response;
                    ServiceException serviceException = null;
                    try
                    {
                        serviceException = _errorResponseHandler.Handle(errorResponse);
                    }
                    catch
                    {
                        throw we;
                    }
                    throw serviceException;
                }
                catch (IOException ex)
                {
                    if (i == Constants.RETRY_TIMES - 1)
                    {
                        throw ex;
                    }
                    Thread.Sleep(1000);
                }
                finally
                {
                    httpResponse?.Close();
                }
            }
            return result;

        }

        /// <summary>
        /// Sets a User-Agent for the specified request, taking into account any custom data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        private void SetUserAgent<T>(IRequest<T> request) where T : KS3Request
        {
            if (!_config.UserAgent.IsNullOrWhiteSpace())
            {
                request.SetHeader(Headers.USER_AGENT, _config.UserAgent);
            }
        }
    }
}
