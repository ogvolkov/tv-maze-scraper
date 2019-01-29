using System;
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
            // arrange 
            int page = 0;

            // act
            List<ShowHeader> shows = await _tvMazeApiClient.GetShowsAsync(page);

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
                async () => await _tvMazeApiClient.GetShowsAsync(unreachablePage)
            );
        
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetsShowCast()
        {
            // arrange
            int showId = 1;

            // act
            List<CastEntry> cast = await _tvMazeApiClient.GetCastAsync(showId);

            // assert
            Assert.That(cast, Is.Not.Null);
            Assert.That(cast.Count, Is.GreaterThan(1));

            Assert.That(cast[0].Person, Is.Not.Null);
            Assert.That(cast[0].Person.Id, Is.EqualTo(1));
            Assert.That(cast[0].Person.Name, Is.EqualTo("Mike Vogel"));
            Assert.That(cast[0].Person.Birthday, Is.EqualTo(new DateTime(1979, 07, 17)));

            Assert.That(cast[1].Person, Is.Not.Null);
            Assert.That(cast[1].Person.Id, Is.EqualTo(2));
            Assert.That(cast[1].Person.Name, Is.EqualTo("Rachelle Lefevre"));
            Assert.That(cast[1].Person.Birthday, Is.EqualTo(new DateTime(1979, 02, 01)));
        }
    }
}
