using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using WeatherAPI;

namespace WeatherAPI.Tests;

public class WeatherForecastEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public WeatherForecastEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task Get_ReturnsOkWithJsonContentType()
    {
        var response = await _client.GetAsync("/WeatherForecast");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Get_ReturnsFiveConsistentForecasts()
    {
        var forecasts = await _client.GetFromJsonAsync<WeatherForecast[]>("/WeatherForecast");

        Assert.NotNull(forecasts);
        Assert.Equal(5, forecasts.Length);
        Assert.All(forecasts, f =>
        {
            Assert.NotNull(f.Summary);
            Assert.Equal(32 + (int)(f.TemperatureC / 0.5556), f.TemperatureF);
        });
    }
}
