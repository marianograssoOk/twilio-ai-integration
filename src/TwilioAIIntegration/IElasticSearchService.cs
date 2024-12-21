using Amazon.Lambda.Core;

namespace TwilioAIIntegration;

public interface IElasticSearchService
{
    Task<string> GetContextFromVectorDbAsync(string query, ILambdaContext lambdaContext);
}
