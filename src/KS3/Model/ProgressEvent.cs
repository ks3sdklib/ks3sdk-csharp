namespace KS3.Model
{
    /// <summary>
    /// Notification of a progress change on an KS3 transfer. 
    /// </summary>
    public class ProgressEvent
    {
        public static int CONTINUE = 0;

        // Single part event codes
        public static int STARTED = 1;
        public static int COMPLETED = 2;
        public static int FAILED = 4;
        public static int CANCELED = 8;
        public static int TRANSFERRED = 16;

        // Multipart event codes
        public static int PART_STARTED = 1024;
        public static int PART_COMPLETED = 2048;
        public static int PART_FAILED = 4096;

        /// <summary>
        /// The number of bytes transferred since the last progress event.
        /// </summary>
        public int BytesTransferred { get; set; }

        /// <summary>
        /// * The unique event code that identifies what type of specific type of event this object represents.
        /// </summary>
        public int EventCode { get; set; }

        public ProgressEvent()
        {

        }

        public ProgressEvent(int eventCode)
        {
            EventCode = eventCode;
        }
    }
}
