using Nest;

namespace TriangleAPI.Interfaces
{
    public interface IElasticSearchService
    {
        public Task<IndexResponse> InsertDocumentAsync<T>(T requestResponse, string indexName);
    }

}
