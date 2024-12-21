using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;

namespace TwilioAIIntegration;

public class StorageService
{
    private readonly IAmazonS3 _s3Client = new AmazonS3Client();
    public async Task SaveConversationToS3(TwilioMessage message, string response, ILambdaContext context)
    {
        var _bucketName = EnvironmentVariables.GetVariable("BUCKET_NAME", context) ?? string.Empty;
        if (string.IsNullOrEmpty(_bucketName)) return;
        
        var key = $"conversations/{DateTime.UtcNow:yyyy-MM-dd}/{Guid.NewGuid()}.json";
        var content = JsonSerializer.Serialize(new { message, response });

        await _s3Client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            ContentBody = content
        });
        context.Logger.LogLine("Conversation saved to S3.");
    }
}