using Amazon.Lambda.Core;

namespace TwilioAIIntegration;

public interface IOpenAIService
{
    Task<string> ProcessMessageAsync(TwilioMessage message, string contextText, ILambdaContext lambdaContext);
}