using Amazon.Lambda.Core;

namespace TwilioAIIntegration;

public interface IEnvironmentVariablesService
{
    string GetVariable(string key, ILambdaContext lambdaContext);
}