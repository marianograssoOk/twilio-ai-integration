using Amazon.Lambda.Core;

namespace TwilioAIIntegration.Tests
{
    public class TestLambdaLogger : ILambdaLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void LogLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
