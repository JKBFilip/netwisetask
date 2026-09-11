namespace CatFactService.Core.Interfaces;

public interface ICloudBackupService
{
    Task AppendFactToCloudAsync(string fact, CancellationToken cancellationToken);
}