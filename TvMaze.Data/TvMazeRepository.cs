
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

        public async Task<IReadOnlyCollection<ShowWithOrderedCast>> GetShowsWithCast(int skip, int take)
        {
            // construct a join instead of _dbContext.Shows.Include(it => it.Cast) because apparently,
            // ordering children on the database level is not that straightforward in EF
            var shows = _dbContext.Shows
                .OrderBy(it => it.Id)
                .Skip(skip)
                .Take(take);

            var showsWithCast =
                from show in shows
                join cast in _dbContext.Cast
                    on show equals cast.Show
                select new { show, cast };

            return await
                showsWithCast
                .OrderBy(it => it.show)
                // NOTE: some cast members have no birth date. assume nulls last is ok
                .ThenByDescending(it => it.cast.Birthday)
                .GroupBy(it => it.show)
                .Select(group =>
                    new ShowWithOrderedCast
                    {
                        Id = group.Key.Id,
                        TvMazeId = group.Key.TvMazeId,
                        Name = group.Key.Name,
                        Cast = group.Select(it => it.cast).ToList()
                    })
                .ToListAsync();
        }
    }
}
