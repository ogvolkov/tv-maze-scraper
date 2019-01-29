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

            await scraper.RunAsync();

            serviceProvider.Dispose();
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
