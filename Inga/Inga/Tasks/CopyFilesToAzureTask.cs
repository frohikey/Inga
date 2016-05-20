using System;
using System.IO;
using System.Linq;
using Inga.Azure.Storage;
using Inga.Log;
using Inga.Tools;

namespace Inga.Tasks
{
    public class CopyFilesToAzureTask : BaseTask
    {
        public CopyFilesToAzureTask(Configuration.Task task, ILogger logger) : base(task, logger)
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

            var files = di.GetFiles("*.*");

            if (files.Length == 0)
            {
                Log($"Directory {Task.In} doesn't contain any files.");
                return;
            }

            var inArchives = files.Select(x => x.Name).ToList().ToArchives();
                        
            var containers = Task.Out.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (containers.Length != 2)
            {
                Log("Out parameter is badly formatted. Use: container;path.");
                return;
            }

            var storage = new Storage(Task.RealConnection);
            var blobs = storage.GetBlobs(containers[0], containers[1]);
            var outArchives = blobs.ToArchives();

            foreach (var ia in inArchives)
            {
                var ex = outArchives.FirstOrDefault(x => x.Filename.Equals(ia.Filename, StringComparison.OrdinalIgnoreCase));

                foreach (var s in ia.Stamps)
                {
                    if (ex == null ||
                        ex.Stamps.All(r => r.Date != s.Date))
                    {
                        var fn = Path.GetFileNameWithoutExtension(ia.Filename) + "_" + TimeStamp.GetStamp(s) + Path.GetExtension(ia.Filename);
                        storage.PutBlob(containers[0], Path.Combine(containers[1], fn), Path.Combine(Task.In, fn));
                        Log($"File {fn} copied to Azure.");
                    }
                }
            }            
        }
    }
}
