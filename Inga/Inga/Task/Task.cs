using Inga.Log;

namespace Inga.Task
{
    public abstract class Task : ITask
    {
        protected Configuration.Task _task;
        protected ILogger _logger;

        public abstract void Run();
        
        protected void Log(string text)
        {
            _logger.Log($"({_task.Name}) {text}");
        }
    }
}
