using CatFactService.Core.Interfaces;
using CatFactService.Core.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CatFactService.Tests;

public class WorkerTests
{
    [Fact]
    public async Task ExecuteAsync_WhenFactReceived_ShouldSaveLocallyAndToCloud()
    {
        var mockLogger = new Mock<ILogger<CatFactService.Worker.Worker>>();
        var mockClient = new Mock<ICatFactClient>();
        var mockStorage = new Mock<IFactStorage>();
        var mockCloud = new Mock<ICloudBackupService>();

        var fakeFact = new CatFactResponse { Fact = "Test fact", Length = 9 };
        mockClient.Setup(c => c.GetRandomFactAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(fakeFact);

        var worker = new CatFactService.Worker.Worker(mockLogger.Object, mockClient.Object, mockStorage.Object, mockCloud.Object);

        await worker.StartAsync(CancellationToken.None);
        await Task.Delay(1200);
        await worker.StopAsync(CancellationToken.None);

        mockStorage.Verify(s => s.SaveFactAsync("Test fact", It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        mockCloud.Verify(c => c.AppendFactToCloudAsync("Test fact", It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }
}