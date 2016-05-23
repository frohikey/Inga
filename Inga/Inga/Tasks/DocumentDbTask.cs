using System;
using System.IO;
using Inga.Azure.DocumentDb;
using Inga.Log;
using Inga.Tools;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace Inga.Tasks
{
    public class DocumentDbTask : BaseTask
    {
        public DocumentDbTask(Configuration.Task task, ILogger logger) : base(task, logger)
        {            
        }

        public override void Run()
        {
            var repo = new Repository<Document>(Task.RealConnection);

            var documents = repo.GetAll();
            var directoryOut = new DirectoryInfo(Task.Out);

            if (!directoryOut.Exists)
                directoryOut.Create();

            var fn = Task.RealConnection.Database + "_" + Task.RealConnection.Collection + "_" + TimeStamp.GetStamp(DateTime.Now) + ".json";
            var outFile = Path.Combine(Task.Out, fn);
            var numberOfDocuments = documents.Count;

            Log($"{numberOfDocuments} documents read.");

            using (var w = File.CreateText(outFile))
            {
                w.WriteLine("[");

                for (var i = 0; i < numberOfDocuments; i++)
                {
                    var document = documents[i];
                    var json = JsonConvert.SerializeObject(document, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new DocumentDbSerializeContractResolver() });                    

                    w.Write(json);

                    if (i != numberOfDocuments - 1)
                        w.WriteLine(",");
                }

                w.WriteLine("]");
            }        

            Log($"{fn} saved.");
        }
    }
}
