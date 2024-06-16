using System.Net;
using FluentAssertions;

namespace AspireWebComponents.Tests.Tests;

public class IntegrationTest
{
    [Fact]
    public async Task GetWeatherApiReturnsOkStatusCode()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AspireWebComponents_AppHost>();
        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        // Act
        var httpClient = app.CreateHttpClient("apiservice");
        var response = await httpClient.GetAsync("/api/weatherforecast");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}