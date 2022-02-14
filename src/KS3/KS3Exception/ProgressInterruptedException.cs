﻿using System;

namespace KS3.KS3Exception
{
    public class ProgressInterruptedException : Exception
    {
        public ProgressInterruptedException(String message) :
            base(message) { }

        public ProgressInterruptedException(String message, Exception cause) :
            base(message, cause) { }
    }
}
