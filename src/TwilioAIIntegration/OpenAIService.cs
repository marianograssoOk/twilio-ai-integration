using Amazon.Lambda.Core;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using OpenAI.Chat;

namespace TwilioAIIntegration;

public class OpenAIService(IEnvironmentVariablesService environmentVariablesService) : IOpenAIService
{
    private const string OpenAiApiKeyParameterName = "OPENAI_API_KEY";
    private const string AwsLambdaFunctionNameEnvVar = "AWS_LAMBDA_FUNCTION_NAME";
    private const string Model = "gpt-4o-mini";

    public async Task<string> ProcessMessageAsync(TwilioMessage message, string contextText, ILambdaContext lambdaContext)
    {
        lambdaContext.Logger.LogLine("Building prompt for Bedrock model...");
        var prompt = $"""
                      
                                  Contexto relevante:
                                  {contextText}
                      
                                  El usuario dice: {message.Body}
                                  Por favor, brinda una respuesta informativa, concisa y útil:
                                  
                      """;

        var apiKey = IsRunningLocally() 
            ? environmentVariablesService.GetVariable(OpenAiApiKeyParameterName, lambdaContext) 
            : await GetOpenAIApiKey();
        
        ChatClient client = new(model: Model, apiKey: apiKey); 
        ChatCompletion chatCompletion = await client.CompleteChatAsync(prompt);
        return chatCompletion.Content.Aggregate(string.Empty, (current, chatMessage) => current + $"{chatMessage.Text}\n");
    }
    
    private static async Task<string> GetOpenAIApiKey()
    {
        var ssmClient = new AmazonSimpleSystemsManagementClient();
        var paramRequest = new GetParameterRequest 
        { 
            Name = OpenAiApiKeyParameterName,
            WithDecryption = true
        };
        var paramResponse = await ssmClient.GetParameterAsync(paramRequest);
        return paramResponse.Parameter.Value;
    }

    private static bool IsRunningLocally()
    {
        return string.IsNullOrEmpty(Environment.GetEnvironmentVariable(AwsLambdaFunctionNameEnvVar));
    }
}