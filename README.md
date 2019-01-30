Create an application that:
1. scrapes the TVMaze API for show and cast information;
2. persists the data in storage;
3. provides the scraped data using a REST API.

The url format would be /api/shows?page={page}, starting from page 1.

Environment:
* .NET Core 2.2.
* Sql Server for storage.
* Scraper is a console app.
* API is an ASP.NET Core app.
* Connection string TVMaze is retrieved from the environment variables.

API decisions:
* When there are no more shows for the page, return an empty collection.
* Assume the ids returned are the ones TVMaze uses, not our internal ones.
* If an actor has no birth date known to TVMaze, put them at the end.
* If an actor plays several roles, return only a single record for the actual physical person.
* Paging starts from 1.
* Page size is 10 for simplicity.

Scraper decisions:
* Save only the data which is explicitly required to display (for simplicity).
* Data retrieval is essentially sequential, as TVMaze rate limiting seems to make the parallelization not very beneficial.
* If an actor plays several roles in a show, save only the single physical person.
* Show to actor is one to many. Oops, that's a blunder - didn't realize person.id applies to all appearances of an actor. Should have been many to many.

Leftovers:
* Save progress when scraping and continue where we left off.
* File-based logs for the scraper.
* Test for scraper's error handling.
* Validate input page parameter in the API.
* Investigate if extra database indices are needed.
* Investigate the best retry and concurrency settings.

Fun fact: TV Maze itself is not fully de-duped, there seem to be actor duplicates, as in http://api.tvmaze.com/shows/7692/cast.