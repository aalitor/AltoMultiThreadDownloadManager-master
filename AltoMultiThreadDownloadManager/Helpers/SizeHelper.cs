using System;

namespace AltoMultiThreadDownloadManager.Helpers
{
    /// <summary>
    /// Provides memory size conversion methods
    /// </summary>
    public static class SizeHelper
    {
        /// <summary>
        /// Converts from bytes unit to best fitted size
        /// </summary>
        /// <param name="byteLen">Amount of bytes to convert</param>
        /// <returns></returns>
        public static string ToHumanReadableSize(this long byteLen)
        {
            string[] sizes = { "Bytes", "Kb", "Mb", "Gb", "Tb" };
            var order = 0;
            double len = byteLen;
            while (len >= 1024d && order < sizes.Length - 1)
            {
                order++;
                len = (len / 1024d);
            }
            return String.Format("{0:0.00} {1}", len, sizes[order]);
        }
        /// <summary>
        /// Converts from bytes unit to best fitted size
        /// </summary>
        /// <param name="byteLen">Amount of bytes to convert</param>
        /// <returns></returns>
        public static string ToHumanReadableSize(this int byteLen)
        {
            return ((long)byteLen).ToHumanReadableSize();
        }

        /// <summary>
        /// Converts from source unit to given unit
        /// </summary>
        /// <param name="value">Size amount to convert</param>
        /// <param name="from">Convert from</param>
        /// <param name="to">Convert to</param>
        /// <returns></returns>
        public static double Convert(this long value, SizeUnit from, SizeUnit to)
        {
            var step = to - from;

            return value / Math.Pow(1024, step);
        }
    }
}