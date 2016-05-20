using System;
using Microsoft.Azure.Documents.Client;

namespace Inga.Azure.DocumentDb
{
    public class Connection
    {
        public DocumentClient Client { get; private set; }

        public string DatabaseId { get; private set; }
        public string CollectionId { get; private set; }

        /// <summary>
        /// Ctor.
        /// </summary>
        public Connection(string endpoint, string authorizationKey, string database, string collection)
        {
            var connectionPolicy = new ConnectionPolicy { UserAgentSuffix = "Inga" };
            Client = new DocumentClient(new Uri(endpoint), authorizationKey, connectionPolicy);
            DatabaseId = database;
            CollectionId = collection;
        }                       
    }
}
