using Amazon.Lambda.Core;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TwilioAIIntegration;

public class TwilioService : ITwilioService
{
    public async Task SendResponse(string from, string to, string message, ILambdaContext context)
    {
        TwilioClient.Init(
            EnvironmentVariables.GetVariable("TWILIO_ACCOUNT_SID", context), 
            EnvironmentVariables.GetVariable("TWILIO_AUTH_TOKEN", context));
        var messageOptions = new CreateMessageOptions(new PhoneNumber(to))
        {
            From = new PhoneNumber(from),
            Body = message
        };

        await MessageResource.CreateAsync(messageOptions);
        context.Logger.LogLine("Message sent to Twilio successfully.");
    }
}

public interface ITwilioService
{
    Task SendResponse(string from, string to, string message, ILambdaContext context);
}