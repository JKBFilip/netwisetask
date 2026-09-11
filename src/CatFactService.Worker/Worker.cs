using CatFactService.Core.Interfaces;

namespace CatFactService.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ICatFactClient _catFactClient;
    private readonly IFactStorage _factStorage;
    private readonly ICloudBackupService _cloudBackup;

    public Worker(ILogger<Worker> logger, ICatFactClient catFactClient, IFactStorage factStorage, ICloudBackupService cloudBackup)
    {
        _logger = logger;
        _catFactClient = catFactClient;
        _factStorage = factStorage;
        _cloudBackup = cloudBackup;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                var response = await _catFactClient.GetRandomFactAsync(stoppingToken);

                if (response is not null && !string.IsNullOrWhiteSpace(response.Fact))
                {
                    await _factStorage.SaveFactAsync(response.Fact, stoppingToken);
                    await _cloudBackup.AppendFactToCloudAsync(response.Fact, stoppingToken);
                    
                    _logger.LogInformation("Fact saved locally and synchronized with Azure ({Length} characters).", response.Length);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching or synchronizing the fact.");
            }
        }
    }
}