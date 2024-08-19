using Nest;
using TriangleAPI.Interfaces;
using TriangleAPI.Services;
using TriangleAPI.Validations;

namespace TriangleAPI.RegisterServiceExtensions
{
    public static class RegisterServiceExtenstion
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<TriangleService>();
            services.AddTransient<TriangleValidator>();
            services.AddElasticsearch(configuration);
            return services;
        }

        public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionSettings = new ConnectionSettings(new Uri(configuration["ReqResElasticSearch:Url"]))
                .BasicAuthentication(configuration["ReqResElasticSearch:User"], configuration["ReqResElasticSearch:Password"]);
            connectionSettings.ThrowExceptions(alwaysThrow: true);
            ElasticClient client = new ElasticClient(connectionSettings);

            services.AddSingleton(client);

            services.AddSingleton<IElasticSearchService, ElasticSearchService>();

        }
    }
}
