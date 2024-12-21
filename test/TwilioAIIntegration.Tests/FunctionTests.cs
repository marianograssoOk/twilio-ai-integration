using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;

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
}
