using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore.WebUtilities;

namespace TwilioAIIntegration
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly IStorageService _storageService;
        private readonly ITwilioService _twilioService;
        private readonly IOpenAIService _openAIService;
        private readonly IElasticSearchService _elasticSearchService;

        public MessageProcessor(IStorageService storageService, IElasticSearchService elasticSearchService, 
            IOpenAIService openAIService, ITwilioService twilioService, IOpenAIService openAiService)
        {
            _storageService = storageService;
            _elasticSearchService = elasticSearchService;
            _twilioService = twilioService;
            _openAIService = openAiService;
        }

        public async Task<string> HandleRequest(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            var parsedQuery = QueryHelpers.ParseQuery(request.Body);
            var message = new TwilioMessage
            {
                From = PhoneNumberHelper.NormalizePhoneNumber(parsedQuery.TryGetValue("From", out var fromValues)
                    ? Uri.UnescapeDataString(fromValues.ToString())
                    : ""),
                To = PhoneNumberHelper.NormalizePhoneNumber(parsedQuery.TryGetValue("To", out var toValues)
                    ? Uri.UnescapeDataString(toValues.ToString())
                    : ""),
                Body = parsedQuery.TryGetValue("Body", out var bodyValues)
                    ? Uri.UnescapeDataString(bodyValues.ToString())
                    : ""
            };

            // Buscar contexto relevante en Elasticsearch
            //var context = await _elasticSearchService.GetContextFromVectorDbAsync(message.Body, lambdaContext);
            var genAIResponse = "prueba"; //await _openAIService.ProcessMessageAsync(message, context, lambdaContext);
            await _storageService.SaveConversationToS3(message, genAIResponse, lambdaContext);
            await _twilioService.SendResponse(message.From, message.To, genAIResponse, lambdaContext);

            return genAIResponse;
        }
    }
}