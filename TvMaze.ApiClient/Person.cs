
using System;

namespace TvMaze.ApiClient
{
    public class Person
    {
        public int Id { get; }

        public string Name { get; }

        public DateTime? Birthday { get; }

        // and so on if we need to store more data than required by the task

        public Person(int id, string name, DateTime? birthday)
        {
            Id = id;
            Name = name;
            Birthday = birthday;
        }
    }
}
