using Nest;
using Newtonsoft.Json;
using TriangleAPI.Interfaces;

namespace TriangleAPI.Services
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly ElasticClient _elasticClient;
        public ElasticSearchService(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<IndexResponse> InsertDocumentAsync<T>(T requestResponse, string indexName)
        {
            IndexResponse ELKResponse = null;

            var indexRequest = new IndexRequest<object>(index: indexName)
            {
                Document = requestResponse
            };

            //Create index if needed
            await CreateIndexIfNeeded(indexName);

            ELKResponse = await _elasticClient.IndexAsync(indexRequest).ConfigureAwait(false);
            return ELKResponse;
        }
        #region create index if need
        private async Task CreateIndexIfNeeded(string indexName)
        {
            var indexExistsResponse = _elasticClient.Indices.Exists(indexName);
            if (!indexExistsResponse.Exists)
            {
                var createIndexResponse = await _elasticClient.Indices.CreateAsync(indexName);
            }
        }
        #endregion
    }


}
