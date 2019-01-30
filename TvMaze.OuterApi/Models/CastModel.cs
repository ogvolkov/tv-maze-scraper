using System;
using Newtonsoft.Json;

namespace TvMaze.OuterApi.Models
{
    public class CastModel
    {
        public int Id { get; }

        public string Name { get; }

        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTime? Birthday { get; }

        public CastModel(int id, string name, DateTime? birthday)
        {
            Id = id;
            Name = name;
            Birthday = birthday;
        }
    }
}
