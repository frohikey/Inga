using System;
using System.Configuration;

namespace Inga.Configuration
{
    public class IngaSection : ConfigurationSection
    {
        [ConfigurationProperty("logFile", IsRequired = true)]
        public string LogFile => this["logFile"] as string;

        [ConfigurationProperty("logType", IsRequired = true)]
        public Enums.LoggerType LogType => (Enums.LoggerType)Enum.Parse(typeof(Enums.LoggerType), this["logType"].ToString());

        public static IngaSection GetConfig()
        {
            return (IngaSection)ConfigurationManager.GetSection("inga") ?? new IngaSection();
        }

        [ConfigurationProperty("tasks")]
        [ConfigurationCollection(typeof(Tasks), AddItemName = "task")]
        public Tasks Tasks
        {
            get
            {
                var o = this["tasks"];
                return o as Tasks;
            }
        }
    }
}
