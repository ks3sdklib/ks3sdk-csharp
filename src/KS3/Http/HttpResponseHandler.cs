using System.Net;

namespace KS3.Http
{
    public interface IHttpResponseHandler<T>
    {
        /// <summary>
        /// Accepts an HTTP response object, and returns an object of type T. Individual implementations may choose to handle the response however they need to, and return any type that they need to.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        T Handle(HttpWebResponse response);
    }
}
