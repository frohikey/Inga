using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Inga.Tools;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace Inga.Azure.DocumentDb
{
    public class Repository<T> where T : Resource, IDynamicMetaObjectProvider
    {
        private readonly Connection _connection;
        
        public Repository(Configuration.Connection connection)
        {
            _connection = new Connection(connection.EndPoint, connection.Key, connection.Database, connection.Collection);
        }

        private static async Task<FeedResponse<T>> QuerySingleDocumentAsync(IDocumentQuery<T> query)
        {
            return await query.ExecuteNextAsync<T>();
        }

        private static async Task<List<T>> QueryMoreDocumentsAsync(IDocumentQuery<T> query)
        {
            var entitiesRetrieved = new List<T>();

            while (query.HasMoreResults)
            {
                var queryResponse = await Core.ExecuteWithRetriesAsync(() => QuerySingleDocumentAsync(query));

                var entities = queryResponse.AsEnumerable();

                if (entities != null)
                    entitiesRetrieved.AddRange(entities);
            }

            return entitiesRetrieved;
        }

        private async Task<List<T>> GetAllAsync()
        {
            var q = _connection.Client.CreateDocumentQuery<T>(UriFactory.CreateDocumentCollectionUri(_connection.DatabaseId, _connection.CollectionId)).AsDocumentQuery();
            var response = await Core.ExecuteWithRetriesAsync(() => QueryMoreDocumentsAsync(q));
            return response;
        }

        /// <summary>
        /// Get list of all entities from query.
        /// </summary>        
        public List<T> GetAll()
        {
            var all = AsyncTools.RunSync(GetAllAsync);
            return all;
        }
    }
}
