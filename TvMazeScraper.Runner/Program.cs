using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TvMaze.ApiClient;

namespace TvMaze.Scraper.Runner
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            var scraper = serviceProvider.GetRequiredService<TvMazeScraper>();

            await RunAsync(scraper);

            serviceProvider.Dispose();
        }

        private static async Task RunAsync(TvMazeScraper scraper)
        {
            // TODO start from the last processed page

            for (int page = 0; ; page++)
            {
                IngestionResult result = await scraper.IngestBatch(page);

                switch (result)
                {
                    case IngestionResult.Success:
                        break;
                    case IngestionResult.NothingToProcess:
                        // it's the last batch, finish
                        return;
                }
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<TvMazeScraper>();

            services.AddSingleton<TvMazeApiClient>();
            services.AddHttpClient<TvMazeApiClient>();

            services.AddLogging(builder => builder.AddConsole());
        }
    }
}
