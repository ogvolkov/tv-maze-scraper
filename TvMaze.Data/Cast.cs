using System;

namespace TvMaze.Data
{
    public class Cast
    {
        public int Id { get; set; }

        public int TvMazeId { get; set; }

        public Show Show { get; set; }

        public string Name { get; set; }

        public DateTime? Birthday { get; set; }

        // and so on if we need to store more data than required by the task
    }
}
