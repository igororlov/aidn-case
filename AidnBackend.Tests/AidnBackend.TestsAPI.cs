using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Text.Json;

public class CalculateEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CalculateEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CalculateEndpoint_ShouldReturnCorrectResult()
    {
        // Create test data
        var measurementRequest = new MeasurementRequest
        {
            Measurements = new List<Measurement>
            {
                new Measurement { Type = "TEMP", Value = 39 },
                new Measurement { Type = "HR", Value = 120 },
                new Measurement { Type = "RR", Value = 15 }
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(measurementRequest), Encoding.UTF8, "application/json");

        // Send POST-request
        var response = await _client.PostAsync("/calculate", content);

        // Assert: check 200 OK
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseString);

        Assert.NotNull(jsonResponse);
        Console.WriteLine("API jsonResponse: " + jsonResponse);
        Assert.Equal(3, jsonResponse.GetProperty("score").GetInt32());
    }
}
