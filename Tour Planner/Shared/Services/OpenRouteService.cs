namespace Shared.Services;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class OpenRouteService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OpenRouteService(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    public async Task<ORSResponse> GetDirectionsAsync(double startLat, double startLng, double endLat, double endLng)
    {
        var url = $"https://api.openrouteservice.org/v2/directions/driving-car?api_key={_apiKey}&start={startLng},{startLat}&end={endLng},{endLat}";
        return await _httpClient.GetFromJsonAsync<ORSResponse>(url);
    }
}

public class ORSResponse
{
    public Route[] Routes { get; set; }

    public class Route
    {
        public Geometry Geometry { get; set; }
    }

    public class Geometry
    {
        public double[][] Coordinates { get; set; }
    }
}