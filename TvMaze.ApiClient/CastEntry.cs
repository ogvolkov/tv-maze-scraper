namespace TvMaze.ApiClient
{
    public class CastEntry
    {
        public Person Person { get; }

        public CastEntry(Person person)
        {
            Person = person;
        }
    }
}
