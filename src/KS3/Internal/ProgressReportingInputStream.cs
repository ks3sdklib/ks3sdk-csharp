using KS3.KS3Exception;
using KS3.Model;
using System.IO;

namespace KS3.Internal
{
    public class ProgressReportingInputStream : Stream
    {
        /// <summary>
        /// The threshold of bytes between notifications.
        /// </summary>
        private static readonly int NOTIFICATION_THRESHOLD = Constants.DEFAULT_STREAM_BUFFER_SIZE;

        /// <summary>
        /// The listener to notify.
        /// </summary>
        private readonly IProgressListener _listener;

        /// <summary>
        ///  The original stream.
        /// </summary>
        private readonly Stream _stream;

        /// <summary>
        /// The number of bytes read that the listener hasn't been notified about yet.
        /// </summary>
        private int _unnotifiedByteCount = 0;

        /// <summary>
        ///  Creates a repeatable input stream based on a file.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="listener"></param>
        public ProgressReportingInputStream(Stream stream, IProgressListener listener)
        {
            _stream = stream;
            _listener = listener;
        }

        private void Notify(int bytesRead)
        {
            _unnotifiedByteCount += bytesRead;
            if (_unnotifiedByteCount >= NOTIFICATION_THRESHOLD)
            {
                Commit();
            }
        }

        private void Commit()
        {
            var e = new ProgressEvent(ProgressEvent.TRANSFERRED)
            {
                BytesTransferred = _unnotifiedByteCount
            };

            _listener.ProgressChanged(e);

            _unnotifiedByteCount = 0;
        }

        public override int ReadByte()
        {
            if (!_listener.AskContinue())
            {
                Commit();
                throw new ProgressInterruptedException("ProgreesReportingInputStream: ReadByte has been interrupted.");
            }

            int data = _stream.ReadByte();
            if (data != -1)
            {
                Notify(1);
            }
            else
            {
                Commit();
            }
            return data;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!_listener.AskContinue())
            {
                Commit();
                throw new ProgressInterruptedException("ProgreesReportingInputStream: Read has been interrupted.");
            }

            int bytesRead = _stream.Read(buffer, offset, count);
            if (bytesRead > 0)
            {
                Notify(bytesRead);
            }
            else
            {
                Commit();
            }
            return bytesRead;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        public override bool CanRead
        {
            get { return _stream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _stream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _stream.CanWrite; }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override long Length
        {
            get { return _stream.Length; }
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public override long Position
        {
            get
            {
                return _stream.Position;
            }
            set
            {
                _stream.Position = value;
            }
        }

        public override void Flush()
        {
            _stream.Flush();

            if (_unnotifiedByteCount > 0)
            {
                Commit();
            }
        }

        public override void Close()
        {
            _stream.Close();

            if (_unnotifiedByteCount > 0)
            {
                Commit();
            }
        }
    }
}
