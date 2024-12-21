using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;


[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace TwilioAIIntegration
{
    public class Function
    {
        private readonly MessageProcessor _messageProcessor = new();

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                var response = await _messageProcessor.HandleRequest(request, context);
                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = JsonSerializer.Serialize(new { reply = response })
                };
            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"Error occurred: {ex.Message}");
                return new APIGatewayProxyResponse
                {
                    StatusCode = 500,
                    Body = $"Exception message: {ex.Message}, Inner: {ex.InnerException?.Message}, Stack Trace: {ex.StackTrace}, RequestBody: {request?.Body}"
                };
            }
        }
    }
}
