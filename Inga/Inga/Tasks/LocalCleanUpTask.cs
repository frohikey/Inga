using System.IO;
using System.Linq;
using Inga.Log;
using Inga.Tools;

namespace Inga.Tasks
{
    public class LocalCleanUpTask : BaseTask
    {
        public LocalCleanUpTask(Configuration.Task task, ILogger logger) : base(task, logger)
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

            if (Task.Retention < 0)
            {
                Log("Retention ignored. Skipping.");
                return;
            }

            var inArchives = files.Select(x => x.Name).ToList().ToArchives();
            
            foreach (var ia in inArchives)
            {
                var stamps = ia.Stamps.OrderBy(x => x);

                if (stamps.Count() > Task.Retention)
                {
                    var badStamps = ia.Stamps.OrderBy(x => x).Take(stamps.Count() - Task.Retention);

                    if (Task.Retention == 0)
                        badStamps = ia.Stamps.ToList();

                    foreach (var bs in badStamps)
                    {
                        var fn = Path.GetFileNameWithoutExtension(ia.Filename) + "_" + TimeStamp.GetStamp(bs) + Path.GetExtension(ia.Filename);

                        File.Delete(Path.Combine(Task.In, fn));
                        Log($"File {fn} deleted from local.");
                    }
                }
            }
        }
    }
}
