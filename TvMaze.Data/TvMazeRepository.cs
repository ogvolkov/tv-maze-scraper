
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IReadOnlyCollection<Show>> GetShowsWithCast(int skip, int take)
        {
            // TODO order children. Too bad EF can't do it directly
            return await _dbContext.Shows
                .OrderBy(it => it.Id)
                .Skip(skip)
                .Take(take)
                .Include(it => it.Cast)
                .ToListAsync();
        }
    }
}
