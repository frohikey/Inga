using System;

namespace Inga.Tools
{
    public static class TimeStamp
    {
        private const string DateTimeFormat = "yyyyMMdd";

        /// <summary>
        /// Get easy timestamp string.
        /// </summary>
        public static string Stamp => DateTime.Now.ToString(DateTimeFormat);

        /// <summary>
        /// Get timestamp for a given date.
        /// </summary>
        public static string GetStamp(DateTime dt)
        {
            return dt.ToString(DateTimeFormat);
        }
    }
}
