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
            AzureCleanUp,
            DocumentDb
        }

        /// <summary>
        /// Logger types.
        /// </summary>
        public enum LoggerType
        {
            SimpleLogFile
        }

        /// <summary>
        /// Connection types.
        /// </summary>
        public enum ConnectionType
        {
            Storage,
            DocumentDb
        }
    }
}
