using Microsoft.EntityFrameworkCore;

namespace TvMaze.Data
{
    public class TvMazeContext : DbContext
    {
        public DbSet<Show> Shows { get; set; }

        public DbSet<Cast> Cast { get; set; }

        public TvMazeContext(DbContextOptions<TvMazeContext> options)
            : base(options)
        {
        }
    }
}
