using CatFactService.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CatFactService.Infrastructure.Storage;

public class LocalFileFactStorage : IFactStorage
{
    private readonly string _filePath;

    public LocalFileFactStorage(IConfiguration configuration)
    {
        var outputDir = configuration.GetSection("LocalSettings:OutputDir").Value ?? "output";
        var fileName = configuration.GetSection("LocalSettings:FileName").Value ?? "catfacts.txt";
        
        Directory.CreateDirectory(outputDir);
        _filePath = Path.Combine(outputDir, fileName);
    }

    public async Task SaveFactAsync(string fact, CancellationToken cancellationToken)
    {
        await File.AppendAllTextAsync(_filePath, fact + Environment.NewLine, cancellationToken);
    }
}