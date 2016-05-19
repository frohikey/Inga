using System;
using Inga.Configuration;
using Inga.Log;
using Inga.Task;

namespace Inga
{
    class Program
    {
        static void Main(string[] args)
        {
            var now = DateTime.Now;
            var config = IngaSection.GetConfig();            
            var logger = LoggerFactory.Create(config);
            
            logger.Log("Session started...");

            foreach (var t in config.Tasks)
            {
                var tt = (Configuration.Task)t;
                var task = TaskFactory.Create(tt, logger);

                try
                {
                    if (tt.On != null &&
                        tt.On.Count > 0)
                    {
                        var anyDay = false;

                        foreach (var d in tt.On)
                        {
                            var day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), d);

                            if (day == now.DayOfWeek)
                            {
                                anyDay = true;
                                break;
                            }
                        }

                        if (!anyDay)
                        {
                            logger.Log($"Task {tt.Name} skipped.");
                            continue;
                        }
                    }

                    logger.Log($"Task {tt.Name} started...");
                    task.Run();
                    logger.Log($"Task {tt.Name} ended.");
                }
                catch (Exception ex)
                {                                        
                    logger.Log($"Running of task {tt.Name} failed.", ex);
                }                
            }

            logger.Log("Session ended.");
        }
    }
}
