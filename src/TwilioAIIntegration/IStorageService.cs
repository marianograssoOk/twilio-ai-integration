using Amazon.Lambda.Core;

namespace TwilioAIIntegration;

public interface IStorageService
{
    Task SaveConversationToS3(TwilioMessage message, string response, ILambdaContext context);
}