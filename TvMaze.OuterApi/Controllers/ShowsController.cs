using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TvMaze.OuterApi.Models;

namespace TvMaze.OuterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private const int PAGE_SIZE = 10;

        [HttpGet]
        public IEnumerable<ShowModel> Get(int? page)
        {
            int pageNumber = page ?? 1;
            int skip = (pageNumber - 1) * PAGE_SIZE;

            return Enumerable.Range(skip, PAGE_SIZE).Select(x => ConvertShow());
        }

        private ShowModel ConvertShow()
        {
            return new ShowModel(10, "TEst", new List<CastModel>
            {
                new CastModel(1, "Mike VOgel", new DateTime(1979, 07, 17)),
                new CastModel(1, "Jack Daniels", new DateTime(1925, 05, 11))
            });
        }
    }
}
