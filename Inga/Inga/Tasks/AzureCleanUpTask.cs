using System;
using System.IO;
using System.Linq;
using Inga.Azure.Storage;
using Inga.Log;
using Inga.Tools;

namespace Inga.Tasks
{
    public class AzureCleanUpTask : BaseTask
    {
        public AzureCleanUpTask(Configuration.Task task, ILogger logger) : base(task, logger)
        {            
        }

        public override void Run()
        {
            var containers = Task.In.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (containers.Length != 2)
            {
                Log("Out parameter is badly formatted. Use: container;path.");
                return;
            }

            if (Task.Retention < 0)
            {
                Log("Retention ignored. Skipping.");
                return;
            }

            var storage = new Storage(Task.RealConnection);
            var blobs = storage.GetBlobs(containers[0], containers[1]);

            if (blobs.Count == 0)
            {
                Log($"Container {containers[0]} doesn't contain any files.");
                return;
            }

            var outArchives = blobs.ToArchives();            

            foreach (var outArchive in outArchives)
            {
                var stamps = outArchive.Stamps.OrderBy(x => x);

                if (stamps.Count() > Task.Retention)
                {
                    var badStamps = outArchive.Stamps.OrderBy(x => x).Take(stamps.Count() - Task.Retention).ToList();

                    if (Task.Retention == 0)
                        badStamps = outArchive.Stamps.ToList();

                    foreach (var bs in badStamps)
                    {
                        var fn = Path.GetFileNameWithoutExtension(outArchive.Filename) + "_" + TimeStamp.GetStamp(bs) + Path.GetExtension(outArchive.Filename);

                        storage.DeleteBlob(containers[0], Path.Combine(containers[1], fn));
                        Log($"File {fn} deleted from Azure.");
                    }
                }
            }
        }
    }
}
