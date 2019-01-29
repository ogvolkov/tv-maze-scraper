namespace TvMaze.ApiClient
{
    public class ShowHeader
    {
        public int Id { get; }

        public string Name { get; }

        public ShowHeader(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
