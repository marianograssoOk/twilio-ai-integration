using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace TwilioAIIntegration
{
    public static class Startup
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<SecretsHolder>()
                .Build();

            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<IMessageProcessor, MessageProcessor>();
            services.AddSingleton<IStorageService, StorageService>();
            services.AddSingleton<IElasticSearchService, ElasticSearchService>();
            services.AddSingleton<IOpenAIService, OpenAIService>();
            services.AddSingleton<ITwilioService, TwilioService>();
            services.AddSingleton<IEnvironmentVariablesService, EnvironmentVariablesService>();

            return services.BuildServiceProvider();
        }
    }

    public class SecretsHolder { }
}