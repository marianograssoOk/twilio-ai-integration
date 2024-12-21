using Amazon.Lambda.Core;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using OpenAI.Chat;

namespace TwilioAIIntegration;

public class OpenAIService
{
    public async Task<string> ProcessMessageAsync(TwilioMessage message, string contextText, ILambdaContext lambdaContext)
    {
        lambdaContext.Logger.LogLine("Building prompt for Bedrock model...");
        var prompt = $"""
                      
                                  Contexto relevante:
                                  {contextText}
                      
                                  El usuario dice: {message.Body}
                                  Por favor, brinda una respuesta informativa, concisa y útil:
                                  
                      """;
            
        ChatClient client = new(model: "gpt-4o-mini", apiKey: await GetOpenAIApiKey(lambdaContext)); 
        ChatCompletion chatCompletion = await client.CompleteChatAsync(prompt);
        return chatCompletion.Content.Aggregate(string.Empty, (current, chatMessage) => current + $"{chatMessage.Text}\n");
    }
    
    private static async Task<string> GetOpenAIApiKey(ILambdaContext lambdaContext)
    {
        var ssmClient = new AmazonSimpleSystemsManagementClient();
        var paramRequest = new GetParameterRequest 
        { 
            Name = "OPENAI_API_KEY",
            WithDecryption = true
        };
        var paramResponse = await ssmClient.GetParameterAsync(paramRequest);
        return paramResponse.Parameter.Value;
    }
}