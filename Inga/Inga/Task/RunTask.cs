using System.Diagnostics;
using System.IO;
using Inga.Log;

namespace Inga.Task
{
    public class RunTask : Task
    {
        public RunTask(Configuration.Task task, ILogger logger)
        {
            _task = task;
            _logger = logger;
        }

        public override void Run()
        {
            var startInfo = new ProcessStartInfo
                            {
                                CreateNoWindow = false,
                                UseShellExecute = true,
                                //WorkingDirectory = Path.GetDirectoryName(_task.In),
                                FileName = _task.In,
                                Arguments = _task.Out,
                                WindowStyle = ProcessWindowStyle.Hidden,                                
                            };

         
            Log($"{Path.GetFileName(_task.In)} process started...");

            using (var exeProcess = Process.Start(startInfo))
            {
                exeProcess.WaitForExit();
            }

            Log($"{Path.GetFileName(_task.In)} process ended.");
        }
    }
}
