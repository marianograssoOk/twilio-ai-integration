using Amazon.Lambda.Core;

namespace TwilioAIIntegration
{
    public static class EnvironmentVariables
    {
        public static string GetVariable(string key, ILambdaContext lambdaContext)
        {
            var environmentVariable = Environment.GetEnvironmentVariable(key);
            if (environmentVariable != null) return environmentVariable;
            
            lambdaContext.Logger.LogError($"Environment variable {key} was not found.");
            return string.Empty;
        }
    }
}
