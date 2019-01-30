using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TvMaze.Data;
using TvMaze.OuterApi.Models;

namespace TvMaze.OuterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private const int PAGE_SIZE = 10;

        private readonly TvMazeRepository _repository;

        public ShowsController(TvMazeRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public async Task<List<ShowModel>> Get(int? page)
        {
            int pageNumber = page ?? 1;
            int skip = (pageNumber - 1) * PAGE_SIZE;

            IReadOnlyCollection<Show> shows = await _repository.GetShowsWithCast(skip, PAGE_SIZE);

            return shows.Select(ConvertShow).ToList();
        }

        private ShowModel ConvertShow(Show show)
        {
            // NOTE: assume the consumer is interested in the original TvMaze ids
            // at least, that would be easier for debugging
            List<CastModel> cast = show.Cast.Select(ConvertCast).ToList();

            return new ShowModel(show.TvMazeId, show.Name, cast);
        }

        private CastModel ConvertCast(Cast cast)
        {
            return new CastModel(cast.TvMazeId, cast.Name, cast.Birthday);
        }
    }
}
