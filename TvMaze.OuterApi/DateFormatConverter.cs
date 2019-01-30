
using Newtonsoft.Json.Converters;

namespace TvMaze.OuterApi
{
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
