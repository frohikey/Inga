using System.IO;
using Inga.Log;
using Inga.Tools;
using Ionic.Zip;
using Ionic.Zlib;

namespace Inga.Task
{
    public class CompressDirectoryTask : Task
    {                
        public CompressDirectoryTask(Configuration.Task task, ILogger logger)
        {
            _logger = logger;
            _task = task;
        }

        public override void Run()
        {
            var di = new DirectoryInfo(_task.In);

            if (!di.Exists)
            {
                Log($"Directory {_task.In} doesn't exist.");
                return;
            }
            
            var od = new DirectoryInfo(_task.Out);

            if (!od.Exists)
                od.Create();

            using (var zip = new ZipFile())
            {
                zip.CompressionLevel = CompressionLevel.BestSpeed;

                zip.AddDirectory(di.FullName);
                var path = Path.Combine(_task.Out, $"{di.Name}_{TimeStamp.Stamp}.zip");
                zip.Save(path);
                Log($"Directory {di.Name} packed.");
            }
        }
    }
}
