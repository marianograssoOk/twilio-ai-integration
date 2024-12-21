using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;

namespace TwilioAIIntegration;

public class StorageService(IEnvironmentVariablesService _environmentVariablesService) : IStorageService
{
    private readonly IAmazonS3 _s3Client = new AmazonS3Client();
    public async Task SaveConversationToS3(TwilioMessage message, string response, ILambdaContext context)
    {
        var content = JsonSerializer.Serialize(new { message, response });

        if (IsRunningLocally()) return;

        var bucketName = _environmentVariablesService.GetVariable("BUCKET_NAME", context);
        if (string.IsNullOrEmpty(bucketName))
        {
            context.Logger.LogLine("Bucket name is not set.");
            return;
        }

        var key = $"conversations/{DateTime.UtcNow:yyyy-MM-dd}/{Guid.NewGuid()}.json";

        try
        {
            await _s3Client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                ContentBody = content
            });
            context.Logger.LogLine("Conversation saved to S3.");
        }
        catch (Exception ex)
        {
            context.Logger.LogLine($"Error saving conversation to S3: {ex.Message}");
        }
    }

    private static bool IsRunningLocally()
    {
        return string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_LAMBDA_FUNCTION_NAME"));
    }
}

public interface IStorageService
{
    Task SaveConversationToS3(TwilioMessage message, string response, ILambdaContext context);
}