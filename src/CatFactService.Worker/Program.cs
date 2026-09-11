using CatFactService.Core.Interfaces;
using CatFactService.Infrastructure.Clients;
using CatFactService.Infrastructure.Storage;
using CatFactService.Worker;
using Polly;
using Polly.Extensions.Http;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IFactStorage, LocalFileFactStorage>();
builder.Services.AddSingleton<ICloudBackupService, AzureBlobBackupService>();

builder.Services.AddHttpClient<ICatFactClient, HttpCatFactClient>(client => 
    {
        var baseUrl = builder.Configuration.GetSection("CatFactApi:BaseUrl").Value ?? "https://catfact.ninja/";
        client.BaseAddress = new Uri(baseUrl);
    })
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();