namespace Inga.Tools
{
    public static class Converters
    {
        /// <summary>
        /// Convert object to nullable int.
        /// </summary>
        public static int? ToInt(this object val)
        {
            if (val == null)
                return null;

            int outValue;

            return int.TryParse(val.ToString(), out outValue) ? (int?)outValue : null;
        }
    }
}
