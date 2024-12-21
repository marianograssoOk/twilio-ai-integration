using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace TwilioAIIntegration.Tests
{
    public class FunctionTests
    {
        [Fact]
        public async Task FunctionHandler_ReturnsExpectedResponse()
        {
            // Arrange
            var function = new Function();
            var request = new APIGatewayProxyRequest
            {
                Resource = "/message",
                Path = "/message",
                HttpMethod = "POST",
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/x-www-form-urlencoded" }
                },
                MultiValueHeaders = new Dictionary<string, IList<string>>
                {
                    { "Content-Type", new List<string> { "application/x-www-form-urlencoded" } }
                },
                Body = "From=+5492215861193&To=+14155238886&Body=Hola como estas?, necesitaria ayuda medica.",
                IsBase64Encoded = false
            };

            var testContext = new TestLambdaContext();

            // Act
            var response = await function.FunctionHandler(request, testContext);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(200, response.StatusCode);

            var responseBody = JsonSerializer.Deserialize<ResponseBody>(response.Body);
            Assert.NotNull(responseBody);
            Assert.Equal("Processed response from Bedrock", responseBody.Reply);
        }

        private class ResponseBody
        {
            public string Reply { get; set; }
        }
    }
    
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
