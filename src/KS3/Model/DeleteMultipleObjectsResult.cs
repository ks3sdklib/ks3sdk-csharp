using System.Collections.Generic;

namespace KS3.Model
{
    public class DeleteMultipleObjectsResult
    {
        /// <summary>
        /// the success delete keys
        /// </summary>
        public IList<string> Deleted { get; set; } = new List<string>();

        /// <summary>
        /// the error delete keys and error message
        /// </summary>
        public IList<DeleteMultipleObjectsError> Errors { get; set; } = new List<DeleteMultipleObjectsError>();

        public void AddDeleteErrors(DeleteMultipleObjectsError error)
        {
            Errors.Add(error);
        }
    }
}
