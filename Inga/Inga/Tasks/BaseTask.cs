using Inga.Log;

namespace Inga.Tasks
{
    public abstract class BaseTask : ITask
    {
        private readonly Configuration.Task _task;
        private readonly ILogger _logger;

        protected Configuration.Task Task =>_task;        

        public abstract void Run();

        protected BaseTask(Configuration.Task task, ILogger logger)
        {
            _task = task;
            _logger = logger;
        }

        protected void Log(string text)
        {
            _logger.Log($"({_task.Name}) {text}");
        }
    }
}
