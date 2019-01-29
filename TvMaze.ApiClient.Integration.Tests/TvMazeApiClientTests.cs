using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TvMaze.ApiClient.Integration.Tests
{
    [TestFixture]
    public class TvMazeApiClientTests
    {
        private TvMazeApiClient _tvMazeApiClient;

        [SetUp]
        public void SetUp()
        {
            _tvMazeApiClient = new TvMazeApiClient(new HttpClient());
        }

        [Test]
        public async Task GetShowsRetrievesFirstPage()
        {
            // act
            List<ShowHeader> shows = await _tvMazeApiClient.GetShows(0);

            // assert
            Assert.That(shows, Is.Not.Null);
            Assert.That(shows.Count, Is.GreaterThan(1));

            // failure here might not necessarily mean the client code is incorrect (the data might have changed)
            // but it's still a good indicator that the scraper's functioning might be threatened
            Assert.That(shows[0].Id, Is.EqualTo(1));
            Assert.That(shows[0].Name, Is.EqualTo("Under the Dome"));

            Assert.That(shows[1].Id, Is.EqualTo(2));
            Assert.That(shows[1].Name, Is.EqualTo("Person of Interest"));
        }

        [Test]
        public void GetShowsRetrieves404ForPagesFarAway()
        {
            // arrange
            int unreachablePage = 99999;

            // act + assert
            UnsuccessfulStatusCodeException exception = Assert.ThrowsAsync<UnsuccessfulStatusCodeException>(
                async () => await _tvMazeApiClient.GetShows(unreachablePage)
            );
        
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
