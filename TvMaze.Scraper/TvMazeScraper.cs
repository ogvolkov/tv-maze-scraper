using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using TvMaze.ApiClient;
using TvMaze.Data;

namespace TvMaze.Scraper
{
    public class TvMazeScraper
    {
        private readonly TvMazeApiClient _apiClient;
        private readonly TvMazeRepository _tvMazeRepository;
        private readonly ILogger<TvMazeScraper> _logger;

        public TvMazeScraper(
            TvMazeApiClient apiClient,
            TvMazeRepository tvMazeRepository,
            ILogger<TvMazeScraper> logger
        )
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _tvMazeRepository = tvMazeRepository ?? throw new ArgumentNullException(nameof(tvMazeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task RunAsync()
        {
            // TODO start from the last processed page

            // make sure the ingestion eventually stops (e.g. if a systematic error prevents us from getting NotFoundException)
            const int PAGE_LIMIT = 99999;

            for (int page = 0; page < PAGE_LIMIT; page++)
            {
                IngestionResult result = await IngestBatch(page);

                switch (result)
                {
                    case IngestionResult.Success:
                        // continue to the next page
                        break;

                    case IngestionResult.NothingToProcess:
                        // it's the last batch, finish
                        _logger.LogInformation("Finished processing all batches");
                        return;

                    case IngestionResult.Failure:
                        // assuming the error is not transient, we can only continue
                        break;
                }
            }

            _logger.LogError("Stopped ingestion as error count exceeded the limit");
        }

        private async Task<IngestionResult> IngestBatch(int page)
        {
            try
            {
                List<ShowHeader> showsBatch = await _apiClient.GetShowsAsync(page);
                _logger.LogInformation("Received page {0} from the API, got {1} show(s)", page, showsBatch.Count);

                // given the TV Maze rate limits, it doesn't make much sense to send requests in parallel, as they get limited pretty soon
                foreach (ShowHeader showHeader in showsBatch)
                {
                    int showId = showHeader.Id;

                    try
                    {
                        List<CastEntry> cast = await _apiClient.GetCastAsync(showId);
                        _logger.LogInformation("Retrieved the cast for show {0}", showId);



                        await SaveShow(showHeader, cast);
                    }
                    catch (Exception exception)
                    {
                        // assume non-transient error has happened, don't stop getting the rest of the data
                        _logger.LogError(exception, "Error when retrieving cast for show {0}", showId);
                    }
                }

                return IngestionResult.Success;
            }
            catch (NotFoundException)
            {
                _logger.LogInformation("Shows for page not found {0}", page);
                return IngestionResult.NothingToProcess;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error when getting page {0} from the API", page);
                return IngestionResult.Failure;
            }
        }

        private async Task SaveShow(ShowHeader showHeader, List<CastEntry> cast)
        {
            try
            {
                IEnumerable<Cast> castDbos = cast.Select(it => new Cast
                {
                    TvMazeId = it.Person.Id,
                    Name = it.Person.Name,
                    Birthday = it.Person.Birthday
                });

                // now, an actor can represent several characters in the show
                // in a full-blown system, we would probably store all the actor: character relationships,
                // but as now we need only people, store de-duped data for simplicity
                List<Cast> uniqueCast = castDbos.Distinct(new PersonEqualityComparer()).ToList();

                var showDbo = new Show
                {
                    TvMazeId = showHeader.Id,
                    Name = showHeader.Name,
                    Cast = uniqueCast
                };

                await _tvMazeRepository.PutShow(showDbo);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Cannot save show {0} into the database", showHeader.Id);
            }
        }

        private class PersonEqualityComparer: IEqualityComparer<Cast>
        {
            public bool Equals(Cast x, Cast y)
            {
                return x.TvMazeId == y.TvMazeId;
            }

            public int GetHashCode(Cast obj)
            {
                return obj.TvMazeId;
            }
        }
    }
}
