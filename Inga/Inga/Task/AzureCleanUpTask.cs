using System;
using System.IO;
using System.Linq;
using Inga.Log;
using Inga.Tools;

namespace Inga.Task
{
    public class AzureCleanUpTask : Task
    {
        public AzureCleanUpTask(Configuration.Task task, ILogger logger)
        {
            _task = task;
            _logger = logger;
        }

        public override void Run()
        {
            var containers = _task.In.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (containers.Length != 2)
            {
                Log("Out parameter is badly formatted. Use: container;path.");
                return;
            }

            if (_task.Retention < 0)
            {
                Log("Retention ignored. Skipping.");
                return;
            }

            var blobs = Azure.Storage.GetBlobs(containers[0], containers[1]);

            if (blobs.Count == 0)
            {
                Log($"Container {containers[0]} doesn't contain any files.");
                return;
            }

            var outArchives = blobs.ToArchives();            

            foreach (var ia in outArchives)
            {
                var stamps = ia.Stamps.OrderBy(x => x);

                if (stamps.Count() > _task.Retention)
                {
                    var badStamps = ia.Stamps.OrderBy(x => x).Take(stamps.Count() - _task.Retention).ToList();

                    if (_task.Retention == 0)
                        badStamps = ia.Stamps.ToList();

                    foreach (var bs in badStamps)
                    {
                        var fn = Path.GetFileNameWithoutExtension(ia.Filename) + "_" + TimeStamp.GetStamp(bs) + Path.GetExtension(ia.Filename);

                        Azure.Storage.DeleteBlob(containers[0], Path.Combine(containers[1], fn));
                        Log($"File {fn} deleted from Azure.");
                    }
                }
            }
        }
    }
}
