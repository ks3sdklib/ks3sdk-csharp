using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    /**
     * Notification of a progress change on an KS3 transfer. 
     */
    public class ProgressEvent
    {
        public static int CONTINUE = 0;

        // Single part event codes
        public static int STARTED     = 1;
        public static int COMPLETED   = 2;
        public static int FAILED      = 4;
        public static int CANCELED    = 8;
        public static int TRANSFERRED = 16;

        // Multipart event codes
        public static int PART_STARTED   = 1024;
        public static int PART_COMPLETED = 2048;
        public static int PART_FAILED    = 4096;

        /** The number of bytes transferred since the last progress event. */
        private int bytesTransferred;

        /**
         * The unique event code that identifies what type of specific type of event
         * this object represents.
         */
        private int eventCode;

        public ProgressEvent(int eventCode)
        {
            this.eventCode = eventCode;
        }

        /**
         * Sets the number of bytes transferred since the last progress event.
         */
        public void setBytesTransferred(int bytesTransferred)
        {
            this.bytesTransferred = bytesTransferred;
        }

        /**
         * Returns the number of bytes transferred since the last progress event.
         */
        public int getBytesTransferred()
        {
            return this.bytesTransferred;
        }

        /**
         * Returns the unique event code that identifies what type of specific type
         * of event this object represents.
         */
        public int getEventCode()
        {
            return this.eventCode;
        }
        
        /**
         * Sets the unique event code that identifies what type of specific type of
         * event this object represents.
         */
        public void setEventCode(int eventType)
        {
            this.eventCode = eventType;
        }
    }
}
