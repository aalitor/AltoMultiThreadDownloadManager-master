using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltoMultiThreadDownloadManager.Enums
{
    /// <summary>
    /// Provides status for resume capability
    /// </summary>
    public enum Resumeability
    {
        /// <summary>
        /// Supports resume
        /// </summary>
        Yes = 1,
        /// <summary>
        /// Does not support resume
        /// </summary>
        No = 0,
        /// <summary>
        /// The resume property is unknown
        /// </summary>
        Unknown = -1
    }
}
