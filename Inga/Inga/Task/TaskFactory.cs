using System;
using Inga.Configuration;
using Inga.Log;

namespace Inga.Task
{
    public static class TaskFactory
    {
        public static ITask Create(Configuration.Task task, ILogger logger)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            switch (task.Type)
            {
                case Enums.TaskType.CompressDirectories:
                    return new CompressDirectoriesTask(task, logger);

                case Enums.TaskType.CompressDirectory:
                    return new CompressDirectoryTask(task, logger);

                case Enums.TaskType.Run:
                    return new RunTask(task, logger);

                case Enums.TaskType.CopyFilesToAzure:
                    return new CopyFilesToAzureTask(task, logger);

                case Enums.TaskType.LocalCleanUp:
                    return new LocalCleanUpTask(task, logger);

                case Enums.TaskType.AzureCleanUp:
                    return new AzureCleanUpTask(task, logger);

                default:
                    throw new Exception($"Task of type {task.Type} cannot be created.");
            }
        }
    }
}
