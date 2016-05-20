using System;
using System.ComponentModel;
using System.Configuration;

namespace Inga.Configuration
{
    public class Task : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name => this["name"] as string;

        [ConfigurationProperty("type", IsRequired = true)]
        public Enums.TaskType Type => (Enums.TaskType)Enum.Parse(typeof(Enums.TaskType), this["type"].ToString());

        [ConfigurationProperty("in", IsRequired = false)]
        public string In => this["in"] as string;

        [ConfigurationProperty("out", IsRequired = false)]
        public string Out => this["out"] as string;

        [ConfigurationProperty("retention", IsRequired = false)]
        public int Retention => int.Parse(this["retention"].ToString());

        [ConfigurationProperty("on", IsRequired = false)]
        [TypeConverter(typeof(CommaDelimitedStringCollectionConverter))]
        public CommaDelimitedStringCollection On => this["on"] as CommaDelimitedStringCollection;

        [ConfigurationProperty("connection", IsRequired = false)]
        public string Connection => this["connection"] as string;

        public Connection RealConnection => IngaSection.GetConfig().Connections[Connection];
    }
}
