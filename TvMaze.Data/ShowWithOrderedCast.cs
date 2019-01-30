using System.Collections.Generic;

namespace TvMaze.Data
{
    public class ShowWithOrderedCast
    {
        public int Id { get; set; }

        public int TvMazeId { get; set; }

        public string Name { get; set; }

        public List<Cast> Cast { get; set; }
    }
}
