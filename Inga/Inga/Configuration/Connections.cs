using System.Configuration;

namespace Inga.Configuration
{
    public class Connections : ConfigurationElementCollection
    {
        public Connection this[int index]
        {
            get { return BaseGet(index) as Connection; }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        public new Connection this[string name]
        {
            get { return (Connection)BaseGet(name); }
            set
            {
                if (BaseGet(name) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(name)));
                }
                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Connection();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Connection)element).Name;
        }
    }
}
