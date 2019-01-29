using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TvMaze.ApiClient
{
    public class TvMazeApiClient
    {
        private const string BASE_URL = "http://api.tvmaze.com";

        private readonly HttpClient _httpClient;

        public TvMazeApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            _httpClient.BaseAddress = new Uri(BASE_URL);
        }

        public async Task<List<ShowHeader>> GetShows(int page)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"/shows?page={page}");

            if (!response.IsSuccessStatusCode)
            {
                throw new UnsuccessfulStatusCodeException(response.StatusCode);
            }

            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<ShowHeader>>(content);
        }
    }
}
