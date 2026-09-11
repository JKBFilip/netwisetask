namespace CatFactService.Core.Interfaces;

public interface IFactStorage
{
    Task SaveFactAsync(string fact, CancellationToken cancellationToken);
}