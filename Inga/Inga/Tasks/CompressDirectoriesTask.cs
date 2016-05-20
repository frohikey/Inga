using System.IO;
using Inga.Log;
using Inga.Tools;
using Ionic.Zip;
using Ionic.Zlib;

namespace Inga.Tasks
{
    public class CompressDirectoriesTask : BaseTask
    {        
        public CompressDirectoriesTask(Configuration.Task task, ILogger logger) : base(task, logger)
        {            
        }

        public override void Run()
        {
            var di = new DirectoryInfo(Task.In);

            if (!di.Exists)
            {
                Log($"Directory {Task.In} doesn't exist.");
                return;
            }

            var directories = di.GetDirectories();

            if (directories.Length == 0)
            {
                Log($"Directory {Task.In} doesn't contain any sub directories.");
                return;
            }

            var od = new DirectoryInfo(Task.Out);

            if (!od.Exists)
                od.Create();

            foreach (var d in directories)
            {
                using (var zip = new ZipFile())
                {
                    zip.CompressionLevel = CompressionLevel.BestSpeed;
                    zip.UseZip64WhenSaving = Zip64Option.AsNecessary;

                    zip.AddDirectory(d.FullName);
                    var path = Path.Combine(Task.Out, $"{d.Name}_{TimeStamp.Stamp}.zip");
                    zip.Save(path);
                    Log($"Directory {d.Name} packed.");
                }
            }
        }        
    }
}
