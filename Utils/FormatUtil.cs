using System;

namespace ZTE.Utils
{
    /// <summary>
    /// Utility class for formatting speed and traffic data
    /// </summary>
    public static class FormatUtil
    {
        /// <summary>
        /// Format speed from bytes/second to human-readable format (B/s, KB/s, MB/s, GB/s)
        /// </summary>
        /// <param name="bytesPerSec">Speed in bytes per second</param>
        /// <returns>Formatted string like "548.7 KB/s"</returns>
        public static string FormatSpeed(long bytesPerSec)
        {
            if (bytesPerSec < 0)
                return "0 B/s";

            double v = bytesPerSec;
            string[] units = { "B/s", "KB/s", "MB/s", "GB/s" };
            int i = 0;

            while (v >= 1024 && i < units.Length - 1)
            {
                v /= 1024;
                i++;
            }

            return $"{v:F1} {units[i]}";
        }

        /// <summary>
        /// Format data size from bytes to human-readable format (B, KB, MB, GB, TB)
        /// </summary>
        /// <param name="bytes">Size in bytes</param>
        /// <returns>Formatted string like "1.23 GB"</returns>
        public static string FormatBytes(long bytes)
        {
            if (bytes < 0)
                return "0 B";

            double v = bytes;
            string[] units = { "B", "KB", "MB", "GB", "TB" };
            int i = 0;

            while (v >= 1024 && i < units.Length - 1)
            {
                v /= 1024;
                i++;
            }

            return $"{v:F2} {units[i]}";
        }

        /// <summary>
        /// Hide middle part of sensitive string (like ICCID)
        /// </summary>
        /// <param name="text">Original text</param>
        /// <param name="showStart">Number of characters to show at start</param>
        /// <param name="showEnd">Number of characters to show at end</param>
        /// <returns>Masked string like "8986***1234"</returns>
        public static string MaskString(string text, int showStart = 4, int showEnd = 4)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= showStart + showEnd)
                return text;

            var start = text.Substring(0, showStart);
            var end = text.Substring(text.Length - showEnd);
            return $"{start}***{end}";
        }
    }
}
