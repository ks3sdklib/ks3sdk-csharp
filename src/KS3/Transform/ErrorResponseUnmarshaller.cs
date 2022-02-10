using KS3.KS3Exception;
using System.IO;
using System.Text;
using System.Xml;

namespace KS3.Transform
{
    public class ErrorResponseUnmarshaller : IUnmarshaller<ServiceException, Stream>
    {
        public ServiceException Unmarshall(Stream inputStream)
        {
            var requestId = string.Empty;
            var errorCode = string.Empty;
            var message = "Unknow error, no response body.";
            StringBuilder currText = new StringBuilder();
            XmlReader xr = XmlReader.Create(new BufferedStream(inputStream));
            while (xr.Read())
            {
                if (xr.NodeType.Equals(XmlNodeType.EndElement))
                {
                    if (xr.Name.Equals("Message"))
                    {
                        message = currText.ToString();
                    }
                    else if (xr.Name.Equals("Code"))
                    {
                        errorCode = currText.ToString();
                    }
                    else if (xr.Name.Equals("RequestId"))
                    {
                        requestId = currText.ToString();
                    }
                    currText.Clear();
                }
                else if (xr.NodeType.Equals(XmlNodeType.Text))
                {
                    currText.Append(xr.Value);
                }

            }

            ServiceException serviceException = new ServiceException(message)
            {
                ErrorCode = errorCode,
                RequestId = requestId,
            };
            return serviceException;
        }
    }
}
