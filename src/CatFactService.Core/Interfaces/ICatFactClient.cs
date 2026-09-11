using CatFactService.Core.Models;

namespace CatFactService.Core.Interfaces;

public interface ICatFactClient
{
    Task<CatFactResponse?> GetRandomFactAsync(CancellationToken cancellationToken);
}