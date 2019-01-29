
using System;

namespace TvMaze.ApiClient
{
    public class Person
    {
        public int Id { get; }

        public string Name { get; }

        public DateTime? Birthday { get; }

        public Person(int id, string name, DateTime? birthday)
        {
            Id = id;
            Name = name;
            Birthday = birthday;
        }
    }
}
