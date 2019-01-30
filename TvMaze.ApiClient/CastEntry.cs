namespace TvMaze.ApiClient
{
    public class CastEntry
    {
        public Person Person { get; }

        // and so on if we need to store more data than required by the task

        public CastEntry(Person person)
        {
            Person = person;
        }
    }
}
