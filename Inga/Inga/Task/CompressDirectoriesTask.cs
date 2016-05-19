using System.IO;
using Inga.Log;
using Inga.Tools;
using Ionic.Zip;
using Ionic.Zlib;

namespace Inga.Task
{
    public class CompressDirectoriesTask : Task
    {        
        public CompressDirectoriesTask(Configuration.Task task, ILogger logger)
        {
            _task = task;
            _logger = logger;
        }

        public override void Run()
        {
            var di = new DirectoryInfo(_task.In);

            if (!di.Exists)
            {
                Log($"Directory {_task.In} doesn't exist.");
                return;
            }

            var directories = di.GetDirectories();

            if (directories.Length == 0)
            {
                Log($"Directory {_task.In} doesn't contain any sub directories.");
                return;
            }

            var od = new DirectoryInfo(_task.Out);

            if (!od.Exists)
                od.Create();

            foreach (var d in directories)
            {
                using (var zip = new ZipFile())
                {
                    zip.CompressionLevel = CompressionLevel.BestSpeed;
                    zip.UseZip64WhenSaving = Zip64Option.AsNecessary;

                    zip.AddDirectory(d.FullName);
                    var path = Path.Combine(_task.Out, $"{d.Name}_{TimeStamp.Stamp}.zip");
                    zip.Save(path);
                    Log($"Directory {d.Name} packed.");
                }
            }
        }        
    }
}
