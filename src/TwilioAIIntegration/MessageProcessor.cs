using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace TwilioAIIntegration
{
    public class MessageProcessor
    {
        private readonly StorageService _storageService = new();
        private readonly TwilioService _twilioService = new();
        private readonly OpenAIService _openAIService = new();

        public async Task<string> HandleRequest(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            var parsedQuery = QueryHelpers.ParseQuery(request.Body);
            var message = new TwilioMessage
            {
                From = parsedQuery.TryGetValue("From", out StringValues fromValues)
                    ? Uri.UnescapeDataString(fromValues.ToString())
                    : "",
                To = parsedQuery.TryGetValue("To", out StringValues toValues)
                    ? Uri.UnescapeDataString(toValues.ToString())
                    : "",
                Body = parsedQuery.TryGetValue("Body", out StringValues bodyValues)
                    ? Uri.UnescapeDataString(bodyValues.ToString())
                    : ""
            };

            var genAIResponse = await _openAIService.ProcessMessageAsync(message, "", lambdaContext);
            await _storageService.SaveConversationToS3(message, genAIResponse, lambdaContext);
            await _twilioService.SendResponse(message.From, message.To, genAIResponse, lambdaContext);

            return genAIResponse;
        }

        
        
        
    }
}