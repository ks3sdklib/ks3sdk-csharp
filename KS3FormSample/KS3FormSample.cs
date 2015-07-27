using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace KS3FormSample
{
    static class KS3FormSample
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /**
             * Please put your Access Key and Secret Key in the KS3Browser first.
             */
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new KS3Browser());
        }
    }
}
