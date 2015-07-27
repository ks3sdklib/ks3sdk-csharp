using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    /**
     * Listener interface for transfer progress events.
     */
    public interface ProgressListener
    {
        /**
         * Called when progress has changed, such as additional bytes transferred,
         * transfer failed, etc.
         */
        void progressChanged(ProgressEvent progressEvent);

        /**
         * Called before to do the progress.
         * Ask whether to continue the progress. 
         */
        bool askContinue();
    }
}
