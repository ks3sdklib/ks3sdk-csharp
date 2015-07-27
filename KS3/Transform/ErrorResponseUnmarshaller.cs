using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

using KS3.KS3Exception;

namespace KS3.Transform
{
    public class ErrorResponseUnmarshaller : Unmarshaller<ServiceException, Stream>
    {
        public ServiceException unmarshall(Stream inputStream)
        {
            String requestId = null;
            String errorCode = null;
            String message = "Unknow error, no response body.";
            ServiceException serviceException = null;

            StringBuilder currText = new StringBuilder();
            XmlReader xr = XmlReader.Create(new BufferedStream(inputStream));
            while (xr.Read())
            {
                if (xr.NodeType.Equals(XmlNodeType.EndElement))
                {
                    if (xr.Name.Equals("Message")) message = currText.ToString();
                    else if (xr.Name.Equals("Code")) errorCode = currText.ToString();
                    else if (xr.Name.Equals("RequestId")) requestId = currText.ToString();

                    currText.Clear();
                }
                else if (xr.NodeType.Equals(XmlNodeType.Text))
                {
                    currText.Append(xr.Value);
                }

            }

            serviceException = new ServiceException(message);
            serviceException.setErrorCode(errorCode);
            serviceException.setRequestId(requestId);

            return serviceException;
        }
    }
}
