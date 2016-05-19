using System;
using System.IO;
using System.Linq;
using Inga.Log;
using Inga.Tools;

namespace Inga.Task
{
    public class MoveToAzureTask : Task
    {
        public MoveToAzureTask(Configuration.Task task, ILogger logger)
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

            var files = di.GetFiles("*.*");

            if (files.Length == 0)
            {
                Log($"Directory {_task.In} doesn't contain any files.");
                return;
            }

            var inArchives = files.Select(x => x.Name).ToList().ToArchives();

            // delete old local files
            foreach (var ia in inArchives)
            {
                var stamps = ia.Stamps.OrderBy(x => x);
                
                if (stamps.Count() > _task.Retention)
                {
                    var badStamps = ia.Stamps.OrderBy(x => x).Take(stamps.Count() - _task.Retention);

                    foreach (var bs in badStamps)
                    {
                        var fn = Path.GetFileNameWithoutExtension(ia.Filename) + "_" + TimeStamp.GetStamp(bs) + Path.GetExtension(ia.Filename);

                        File.Delete(Path.Combine(_task.In, fn));
                        Log($"File {fn} deleted from local.");
                    }                    
                }
            }

            // refresh files
            files = di.GetFiles("*.*");

            if (files.Length == 0)
            {
                Log($"Directory {_task.In} doesn't contain any files.");
                return;
            }

            inArchives = files.Select(x => x.Name).ToList().ToArchives();

            // get blobs
            var containers = _task.Out.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (containers.Length != 2)
            {
                Log("Out parameter is badly formatted. Use: container;path.");
                return;
            }

            // upload blobs
            var blobs = Azure.Storage.GetBlobs(containers[0], containers[1]);
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
                        Azure.Storage.PutBlob(containers[0], Path.Combine(containers[1], fn), Path.Combine(_task.In, fn));
                        Log($"File {fn} archived to cloud.");
                    }
                }
            }

            // delete old blobs
            blobs = Azure.Storage.GetBlobs(containers[0], containers[1]);
            outArchives = blobs.ToArchives();

            foreach (var ia in outArchives)
            {
                var stamps = ia.Stamps.OrderBy(x => x);

                if (stamps.Count() > _task.Retention)
                {
                    var badStamps = ia.Stamps.OrderBy(x => x).Take(stamps.Count() - _task.Retention);

                    foreach (var bs in badStamps)
                    {
                        var fn = Path.GetFileNameWithoutExtension(ia.Filename) + "_" + TimeStamp.GetStamp(bs) + Path.GetExtension(ia.Filename);

                        Azure.Storage.DeleteBlob(containers[0], Path.Combine(containers[1], fn));                        
                        Log($"File {fn} deleted from cloud.");
                    }
                }
            }
        }
    }
}
