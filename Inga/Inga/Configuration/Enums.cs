namespace Inga.Configuration
{
    public static class Enums
    {
        /// <summary>
        /// Task types.
        /// </summary>
        public enum TaskType
        {
            CompressDirectories,
            CompressDirectory,
            Run,
            MoveToAzure
        }

        /// <summary>
        /// Logger types.
        /// </summary>
        public enum LoggerType
        {
            SimpleLogFile
        }
    }
}
