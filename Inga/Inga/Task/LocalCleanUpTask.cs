using System.IO;
using System.Linq;
using Inga.Log;
using Inga.Tools;

namespace Inga.Task
{
    public class LocalCleanUpTask : Task
    {
        public LocalCleanUpTask(Configuration.Task task, ILogger logger)
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

            if (_task.Retention < 0)
            {
                Log("Retention ignored. Skipping.");
                return;
            }

            var inArchives = files.Select(x => x.Name).ToList().ToArchives();
            
            foreach (var ia in inArchives)
            {
                var stamps = ia.Stamps.OrderBy(x => x);

                if (stamps.Count() > _task.Retention)
                {
                    var badStamps = ia.Stamps.OrderBy(x => x).Take(stamps.Count() - _task.Retention);

                    if (_task.Retention == 0)
                        badStamps = ia.Stamps.ToList();

                    foreach (var bs in badStamps)
                    {
                        var fn = Path.GetFileNameWithoutExtension(ia.Filename) + "_" + TimeStamp.GetStamp(bs) + Path.GetExtension(ia.Filename);

                        File.Delete(Path.Combine(_task.In, fn));
                        Log($"File {fn} deleted from local.");
                    }
                }
            }
        }
    }
}
