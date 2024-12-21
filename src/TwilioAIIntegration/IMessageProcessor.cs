using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace TwilioAIIntegration;

public interface IMessageProcessor
{
    Task<string> HandleRequest(APIGatewayProxyRequest request, ILambdaContext lambdaContext);
}