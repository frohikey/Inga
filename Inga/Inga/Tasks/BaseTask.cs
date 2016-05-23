using Inga.Log;

namespace Inga.Tasks
{
    public abstract class BaseTask : ITask
    {
        private readonly ILogger _logger;

        protected Configuration.Task Task { get; }

        public abstract void Run();

        protected BaseTask(Configuration.Task task, ILogger logger)
        {
            Task = task;
            _logger = logger;
        }

        protected void Log(string text)
        {
            _logger.Log($"({Task.Name}) {text}");
        }
    }
}
