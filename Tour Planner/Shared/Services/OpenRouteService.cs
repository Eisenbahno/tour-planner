namespace Shared.Service
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json.Linq;

    public class OpenRouteService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _apiKey;

        public OpenRouteService(HttpClient httpClient, string? configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration;
        }

        public async Task<string> GetRouteAsync(double startLat, double startLng, double endLat, double endLng)
        {
            var requestUrl =
                $"v2/directions/driving-car?api_key={_apiKey}&start={startLng},{startLat}&end={endLng},{endLat}";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }
}