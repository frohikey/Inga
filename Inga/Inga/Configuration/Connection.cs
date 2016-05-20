using System;
using System.Configuration;

namespace Inga.Configuration
{
    public class Connection : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name => this["name"] as string;

        [ConfigurationProperty("type", IsRequired = true)]
        public Enums.ConnectionType Type => (Enums.ConnectionType)Enum.Parse(typeof(Enums.ConnectionType), this["type"].ToString());

        [ConfigurationProperty("endPoint", IsRequired = true)]
        public string EndPoint => this["endPoint"] as string;

        [ConfigurationProperty("key", IsRequired = true)]
        public string Key => this["key"] as string;

        [ConfigurationProperty("database", IsRequired = false)]
        public string Database => this["database"] as string;

        [ConfigurationProperty("collection", IsRequired = false)]
        public string Collection => this["collection"] as string;

        [ConfigurationProperty("account", IsRequired = false)]
        public string Account => this["account"] as string;
    }
}
