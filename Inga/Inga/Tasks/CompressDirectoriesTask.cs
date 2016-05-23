using System;
using System.IO;
using Inga.Log;
using Inga.Tools;
using Ionic.Zip;
using Ionic.Zlib;
using System.Linq;

namespace Inga.Tasks
{
    public class CompressDirectoriesTask : BaseTask
    {        
        public CompressDirectoriesTask(Configuration.Task task, ILogger logger) : base(task, logger)
        {            
        }

        public override void Run()
        {
            var directoryIn = new DirectoryInfo(Task.In);

            if (!directoryIn.Exists)
            {
                Log($"Directory {Task.In} doesn't exist.");
                return;
            }

            var directories = directoryIn.GetDirectories();

            if (directories.Length == 0)
            {
                Log($"Directory {Task.In} doesn't contain any sub directories.");
                return;
            }

            var directoryOut = new DirectoryInfo(Task.Out);

            if (!directoryOut.Exists)
                directoryOut.Create();

            foreach (var d in directories)
            {
                if (Task.Skip != null &&
                    Task.Skip.Cast<string>().Any(skip => skip.Equals(d.Name, StringComparison.OrdinalIgnoreCase)))
                    continue;

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
