using System.Text;
using Amazon.Kendra;
using Amazon.Kendra.Model;
using Amazon.Lambda.Core;

namespace TwilioAIIntegration;

[Obsolete("Obsolete until I have money")]
public class AmazonKendraService
{
    public async Task<string> GetContextFromVectorDbAsync(string userInput, ILambdaContext context)
    {
        context.Logger.LogLine($"Querying Kendra with user input: {userInput}");

        var kendraClient = new AmazonKendraClient();
        var request = new QueryRequest
        {
            IndexId = Environment.GetEnvironmentVariable("KENDRA_INDEX_ID"),
            QueryText = "dieta personal",//userInput,
            PageSize = 3, // Número de documentos más relevantes
            AttributeFilter = new AttributeFilter
            {
                // Opcional: Filtrar solo documentos de nutrición/dietass
                EqualsTo = new DocumentAttribute
                {
                    Key = "_source_uri",
                    Value = new DocumentAttributeValue { StringValue = "nutricion" }
                }
            }
        };

        try 
        {
            var response = await kendraClient.QueryAsync(request);
        
            // Construir un contexto consolidado de los resultados
            var contextBuilder = new StringBuilder();
            contextBuilder.AppendLine("Información relevante de tus documentos:");

            foreach (var resultItem in response.ResultItems.Where(resultItem => !string.IsNullOrEmpty(resultItem.DocumentExcerpt?.Text)))
            {
                contextBuilder.AppendLine($"- {resultItem.DocumentExcerpt.Text}");
            }

            context.Logger.LogLine($"Kendra context retrieved: {contextBuilder}");
            return contextBuilder.ToString();
        }
        catch (Exception ex)
        {
            context.Logger.LogLine($"Error querying Kendra: {ex.Message}");
            return "No se pudo obtener contexto específico.";
        }
    }
}