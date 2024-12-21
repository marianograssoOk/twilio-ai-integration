using Amazon.Lambda.Core;

namespace TwilioAIIntegration;

using Elastic.Clients.Elasticsearch;

public class ElasticSearchService : IElasticSearchService
{
    private readonly ElasticsearchClient _client;

    public ElasticSearchService()
    {
        var settings = new ElasticsearchClientSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("food_data"); // Cambia "messages" por el índice que uses
        _client = new ElasticsearchClient(settings);
    }

    public async Task<string> GetContextFromVectorDbAsync(string query, ILambdaContext lambdaContext)
    {
        var searchResponse = await _client.SearchAsync<dynamic>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field("body") // Campo del documento en el índice
                        .Query(query)
                    )
                )
                .Size(1) // Limitar al resultado más relevante
        );

        if (!searchResponse.IsValidResponse || searchResponse.Hits.Count == 0)
        {
            lambdaContext.Logger.LogLine("No relevant context found in Elasticsearch.");
            return string.Empty;
        }

        // Obtener el campo más relevante (asumiendo que `body` contiene el texto)
        var topResult = searchResponse.Hits.First().Source as IDictionary<string, object>;
        return topResult != null && topResult.TryGetValue("body", out var context)
            ? context.ToString()
            : string.Empty;
    }
}

public interface IElasticSearchService
{
    Task<string> GetContextFromVectorDbAsync(string query, ILambdaContext lambdaContext);
}
