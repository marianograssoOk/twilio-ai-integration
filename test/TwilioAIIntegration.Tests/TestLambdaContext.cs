using Amazon.Lambda.Core;

namespace TwilioAIIntegration.Tests
{
    public class TestLambdaContext : ILambdaContext
    {
        public string AwsRequestId => "test-request-id";
        public IClientContext ClientContext => null;
        public string FunctionName => "TestFunction";
        public string FunctionVersion => "1";
        public ICognitoIdentity Identity => null;
        public string InvokedFunctionArn => "arn:aws:lambda:us-west-2:123456789012:function:TestFunction";
        public ILambdaLogger Logger => new TestLambdaLogger();
        public string LogGroupName => "/aws/lambda/TestFunction";
        public string LogStreamName => "2024/01/01/[1]abcdef123456";
        public int MemoryLimitInMB => 256;
        public TimeSpan RemainingTime => TimeSpan.FromMinutes(5);
    }
}
