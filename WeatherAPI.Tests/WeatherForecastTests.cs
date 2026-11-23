namespace WeatherAPI.Tests;

public class WeatherForecastTests
{
    [Theory]
    [InlineData(0, 32)]
    [InlineData(100, 211)]
    [InlineData(-20, -3)]
    [InlineData(37, 98)]
    public void TemperatureF_ConvertsFromCelsius(int celsius, int expectedFahrenheit)
    {
        var forecast = new WeatherForecast { TemperatureC = celsius };

        Assert.Equal(expectedFahrenheit, forecast.TemperatureF);
    }

    [Fact]
    public void Properties_RoundTrip()
    {
        var date = new DateOnly(2026, 11, 23);

        var forecast = new WeatherForecast
        {
            Date = date,
            TemperatureC = 21,
            Summary = "Mild"
        };

        Assert.Equal(date, forecast.Date);
        Assert.Equal(21, forecast.TemperatureC);
        Assert.Equal("Mild", forecast.Summary);
    }

    [Fact]
    public void Summary_DefaultsToNull()
    {
        Assert.Null(new WeatherForecast().Summary);
    }
}
