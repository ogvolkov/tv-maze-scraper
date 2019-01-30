namespace TvMaze.ApiClient
{
    public class ShowHeader
    {
        public int Id { get; }

        public string Name { get; }

        // and so on if we need to store more data than required by the task

        public ShowHeader(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
