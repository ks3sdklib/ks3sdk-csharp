using KS3.Http;
using KS3.Model;
using System.IO;
using System.Net;

namespace KS3.Internal
{
    public class ObjectResponseHandler : IHttpResponseHandler<KS3Object>
    {
        private readonly GetObjectRequest _getObjectRequest;

        public ObjectResponseHandler(GetObjectRequest getObjectRequest)
        {
            _getObjectRequest = getObjectRequest;
        }

        public KS3Object Handle(HttpWebResponse response)
        {
            KS3Object ks3Object = new KS3Object();

            FileInfo destinationFile = _getObjectRequest.getDestinationFile();
            byte[] content = null;

            IProgressListener progressListener = this._getObjectRequest.getProgressListener();

            ObjectMetadata metadata = new ObjectMetadata();
            RestUtils.PopulateObjectMetadata(response, metadata);
            ks3Object.Metadata = metadata;

            Stream input = null, output = null;

            try
            {
                input = response.GetResponseStream();

                if (progressListener != null)
                {
                    input = new ProgressReportingInputStream(input, progressListener);
                }
                var bufferSize = Constants.DEFAULT_STREAM_BUFFER_SIZE;
                var buf = new byte[bufferSize];

                if (destinationFile != null)
                {
                    output = new FileStream(_getObjectRequest.getDestinationFile().FullName, FileMode.Create);
                }
                else
                {
                    content = new byte[metadata.GetContentLength()];
                    output = new MemoryStream(content);
                }

                for (; ; )
                {
                    int size = input.Read(buf, 0, bufferSize);
                    if (size <= 0) break;
                    output.Write(buf, 0, size);
                }
            }
            finally
            {
                input?.Close();
                input?.Dispose();

                output?.Close();
                output?.Dispose();
            }

            if (destinationFile != null)
            {
                ks3Object.ObjectContent = destinationFile.OpenRead();
            }
            else
            {
                ks3Object.ObjectContent = new MemoryStream(content);
            }
            return ks3Object;
        }
    }
}
