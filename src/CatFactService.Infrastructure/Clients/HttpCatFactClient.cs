using System.Net.Http.Json;
using CatFactService.Core.Interfaces;
using CatFactService.Core.Models;

namespace CatFactService.Infrastructure.Clients;

public class HttpCatFactClient : ICatFactClient
{
    private readonly HttpClient _httpClient;

    public HttpCatFactClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CatFactResponse?> GetRandomFactAsync(CancellationToken cancellationToken)
    {
        return await _httpClient.GetFromJsonAsync<CatFactResponse>("fact", cancellationToken);
    }
}