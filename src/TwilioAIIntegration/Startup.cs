using Microsoft.Extensions.DependencyInjection;

namespace TwilioAIIntegration
{
    public static class Startup
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IMessageProcessor, MessageProcessor>();
            services.AddSingleton<IStorageService, StorageService>();
            services.AddSingleton<IElasticSearchService, ElasticSearchService>();
            services.AddSingleton<IOpenAIService, OpenAIService>();
            services.AddSingleton<ITwilioService, TwilioService>();
            return services.BuildServiceProvider();
        }
    }
}