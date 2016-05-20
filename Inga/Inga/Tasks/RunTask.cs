using System.Diagnostics;
using System.IO;
using Inga.Log;

namespace Inga.Tasks
{
    public class RunTask : BaseTask
    {
        public RunTask(Configuration.Task task, ILogger logger) : base(task, logger)
        {            
        }

        public override void Run()
        {
            var startInfo = new ProcessStartInfo
                            {
                                CreateNoWindow = false,
                                UseShellExecute = true,
                                //WorkingDirectory = Path.GetDirectoryName(Task.In),
                                FileName = Task.In,
                                Arguments = Task.Out,
                                WindowStyle = ProcessWindowStyle.Hidden,                                
                            };

         
            Log($"{Path.GetFileName(Task.In)} process started...");

            using (var exeProcess = Process.Start(startInfo))
            {
                exeProcess?.WaitForExit();
            }

            Log($"{Path.GetFileName(Task.In)} process ended.");
        }
    }
}
