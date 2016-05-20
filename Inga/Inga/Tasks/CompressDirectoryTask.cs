using System.IO;
using Inga.Log;
using Inga.Tools;
using Ionic.Zip;
using Ionic.Zlib;

namespace Inga.Tasks
{
    public class CompressDirectoryTask : BaseTask
    {                
        public CompressDirectoryTask(Configuration.Task task, ILogger logger) : base(task, logger)
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
            
            var od = new DirectoryInfo(Task.Out);

            if (!od.Exists)
                od.Create();

            using (var zip = new ZipFile())
            {
                zip.CompressionLevel = CompressionLevel.BestSpeed;
                zip.UseZip64WhenSaving = Zip64Option.AsNecessary;

                zip.AddDirectory(di.FullName);
                var path = Path.Combine(Task.Out, $"{di.Name}_{TimeStamp.Stamp}.zip");
                zip.Save(path);
                Log($"Directory {di.Name} packed.");
            }
        }
    }
}
