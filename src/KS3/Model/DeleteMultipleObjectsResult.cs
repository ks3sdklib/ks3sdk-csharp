using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class DeleteMultipleObjectsResult
    {
        /// <summary>
        /// the success delete keys
        /// </summary>
        private IList<String> deleted = new List<String>();

        public IList<String> Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }
        /// <summary>
        /// the error delete keys and error message
        /// </summary>
        private IList<DeleteMultipleObjectsError> errors = new List<DeleteMultipleObjectsError>();

        public IList<DeleteMultipleObjectsError> Errors
        {
            get { return errors; }
            set { errors = value; }
        }
        public void addDeleteErrors(DeleteMultipleObjectsError error) {
            this.errors.Add(error);
        }
    }
}
