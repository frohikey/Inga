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
            CopyFilesToAzure,
            LocalCleanUp,
            AzureCleanUp
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
