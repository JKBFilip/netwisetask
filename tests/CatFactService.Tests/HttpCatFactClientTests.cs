using System.Net;
using System.Text.Json;
using CatFactService.Core.Models;
using CatFactService.Infrastructure.Clients;
using Moq;
using Moq.Protected;
using Xunit;

namespace CatFactService.Tests;

public class HttpCatFactClientTests
{
    [Fact]
    public async Task GetRandomFactAsync_ReturnsFact_WhenApiCallIsSuccessful()
    {
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var fakeResponse = new CatFactResponse { Fact = "Koty są super", Length = 13 };

        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(fakeResponse))
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://catfact.ninja/")
        };

        var client = new HttpCatFactClient(httpClient);
        var result = await client.GetRandomFactAsync(CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Koty są super", result?.Fact);
        Assert.Equal(13, result?.Length);
    }
}