
using System;
using System.Threading.Tasks;

namespace TvMaze.Data
{
    public class TvMazeRepository
    {
        private readonly TvMazeContext _dbContext;

        public TvMazeRepository(TvMazeContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task PutShow(Show show)
        {
            // perhaps it would make sense to perform an idempotent create or update later
            _dbContext.Shows.Add(show);
            await _dbContext.SaveChangesAsync();
        }
    }
}
