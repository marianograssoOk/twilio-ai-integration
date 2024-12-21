using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;

namespace TwilioAIIntegration
{
    public class EnvironmentVariablesService(IConfiguration configuration) : IEnvironmentVariablesService
    {
        public string GetVariable(string key, ILambdaContext lambdaContext)
        {
            var environmentVariable = configuration[key];
            if (environmentVariable != null) return environmentVariable;
            
            lambdaContext.Logger.LogError($"Environment variable {key} was not found.");
            return string.Empty;
        }
    }
}
