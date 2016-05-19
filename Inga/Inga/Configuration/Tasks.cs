using System.Configuration;

namespace Inga.Configuration
{
    public class Tasks : ConfigurationElementCollection
    {
        public Task this[int index]
        {
            get { return BaseGet(index) as Task; }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        public new Task this[string name]
        {
            get { return (Task)BaseGet(name); }
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
            return new Task();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Task)element).Name;
        }
    }
}
