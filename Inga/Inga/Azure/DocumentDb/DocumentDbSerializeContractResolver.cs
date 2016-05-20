using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Inga.Azure.DocumentDb
{
    public class DocumentDbSerializeContractResolver : DefaultContractResolver
    {
        private static readonly string[] _documentDbInternalFields = { "_rid", "_ts", "_self", "_etag", "_attachments" };        

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            property.ShouldSerialize = instance => _documentDbInternalFields.All(f => !f.Equals(property.PropertyName));            

            return property; 
        }
    }
}
