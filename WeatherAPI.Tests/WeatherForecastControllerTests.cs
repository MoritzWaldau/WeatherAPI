using WeatherAPI.Controllers;

namespace WeatherAPI.Tests;

public class WeatherForecastControllerTests
{
    private static readonly string[] ValidSummaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private static WeatherForecast[] GetForecasts() =>
        new WeatherForecastController().Get().ToArray();

    [Fact]
    public void Get_ReturnsFiveItems()
    {
        Assert.Equal(5, GetForecasts().Length);
    }

    [Fact]
    public void Get_ReturnsConsecutiveDatesStartingTomorrow()
    {
        var before = DateOnly.FromDateTime(DateTime.Now);
        var forecasts = GetForecasts();
        var after = DateOnly.FromDateTime(DateTime.Now);

        for (var i = 0; i < forecasts.Length; i++)
        {
            var offset = i + 1;
            // Tolerate a midnight rollover while Get() was running.
            Assert.InRange(forecasts[i].Date, before.AddDays(offset), after.AddDays(offset));
        }

        // Consecutive: each date is exactly one day after the previous one.
        for (var i = 1; i < forecasts.Length; i++)
        {
            Assert.Equal(forecasts[i - 1].Date.AddDays(1), forecasts[i].Date);
        }
    }

    [Fact]
    public void Get_TemperaturesAreWithinRange()
    {
        // Repeat to exercise the randomness; only ranges are asserted.
        for (var run = 0; run < 50; run++)
        {
            foreach (var forecast in GetForecasts())
            {
                Assert.InRange(forecast.TemperatureC, -20, 54);
            }
        }
    }

    [Fact]
    public void Get_SummariesAreNonNullAndKnown()
    {
        for (var run = 0; run < 50; run++)
        {
            foreach (var forecast in GetForecasts())
            {
                Assert.NotNull(forecast.Summary);
                Assert.Contains(forecast.Summary, ValidSummaries);
            }
        }
    }
}
