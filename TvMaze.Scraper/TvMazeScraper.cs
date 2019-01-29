using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using TvMaze.ApiClient;

namespace TvMaze.Scraper
{
    public class TvMazeScraper
    {
        private readonly TvMazeApiClient _apiClient;
        private readonly ILogger<TvMazeScraper> _logger;

        public TvMazeScraper(TvMazeApiClient apiClient, ILogger<TvMazeScraper> logger)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task IngestBatch(int page)
        {
            List<ShowHeader> showsBatch = await _apiClient.GetShowsAsync(page);
            _logger.LogInformation("Received page {0} from the API, got {1} show(s)", page, showsBatch.Count);
        }
    }
}
