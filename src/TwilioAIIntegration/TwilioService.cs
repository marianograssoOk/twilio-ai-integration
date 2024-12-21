using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TwilioAIIntegration;

public class TwilioService(IEnvironmentVariablesService environmentVariablesService) : ITwilioService
{
    private const string TwilioAccountSid = "TWILIO_ACCOUNT_SID";
    private const string TwilioAuthToken = "TWILIO_AUTH_TOKEN";
    public async Task SendResponse(string from, string to, string message, ILambdaContext context)
    {
        try
        {
            TwilioClient.Init(
                environmentVariablesService.GetVariable(TwilioAccountSid, context), 
                environmentVariablesService.GetVariable(TwilioAuthToken, context));
            
            var messageOptions = new CreateMessageOptions(new PhoneNumber($"whatsapp:{from}"))
            {
                From = new PhoneNumber($"whatsapp:{to}"),
                Body = message
            };

            await MessageResource.CreateAsync(messageOptions);

            context.Logger.LogLine("Message sent to Twilio successfully.");
        }
        catch (ApiException ex) when (ex.Code == 21608)
        {
            context.Logger.LogLine($"Error: {ex.Message} - The number {to} might be unverified if using a trial account.");
        }
        catch (ApiException ex) when (ex.Code == 21408)
        {
            context.Logger.LogLine($"Error: {ex.Message} - SMS permissions are not enabled for region of {to}.");
        }
        catch (Exception ex)
        {
            context.Logger.LogLine($"An unexpected error occurred: {ex.Message}");
        }
    }
}

public interface ITwilioService
{
    Task SendResponse(string from, string to, string message, ILambdaContext context);
}