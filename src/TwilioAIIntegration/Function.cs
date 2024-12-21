using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace TwilioAIIntegration;

public class Function(IServiceProvider serviceProvider)
{
    public Function() : this(Startup.ConfigureServices())
    {
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        using var scope = serviceProvider.CreateScope();
        var messageProcessor = scope.ServiceProvider.GetRequiredService<IMessageProcessor>();
        try
        {
            var response = await messageProcessor.HandleRequest(request, context);
            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = JsonSerializer.Serialize(new { reply = response })
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogLine($"Error occurred: {ex.Message}");
            context.Logger.LogLine($"Exception details: {ex}");
            return new APIGatewayProxyResponse
            {
                StatusCode = 500,
                Body = $"Exception message: {ex.Message}, Inner: {ex.InnerException?.Message}, Stack Trace: {ex.StackTrace}, RequestBody: {request?.Body}"
            };
        }
    }
}