using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TvMaze.ApiClient;
using TvMaze.Data;

namespace TvMaze.Scraper.Runner
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddEnvironmentVariables();
            IConfigurationRoot configuration = builder.Build();

            var services = new ServiceCollection();
            ConfigureServices(services, configuration);

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            var scraper = serviceProvider.GetRequiredService<TvMazeScraper>();

            await scraper.RunAsync();

            serviceProvider.Dispose();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<TvMazeScraper>();

            services.AddTransient<TvMazeRepository>();

            services.AddTransient<TvMazeApiClient>();
            services.AddHttpClient<TvMazeApiClient>();

            services.AddDbContext<TvMazeContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("TvMaze"))
            );

            services.AddLogging(builder => builder.AddConsole());
        }
    }
}
