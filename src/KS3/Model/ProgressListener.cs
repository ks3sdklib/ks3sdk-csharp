namespace KS3.Model
{
    /// <summary>
    /// Listener interface for transfer progress events.
    /// </summary>
    public interface IProgressListener
    {
        /// <summary>
        ///  Called when progress has changed, such as additional bytes transferred,transfer failed, etc.
        /// </summary>
        /// <param name="progressEvent"></param>
        void ProgressChanged(ProgressEvent progressEvent);

        /// <summary>
        /// Called before to do the progress. Ask whether to continue the progress.
        /// </summary>
        /// <returns></returns>
        bool AskContinue();
    }
}
