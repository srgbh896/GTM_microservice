using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

[assembly: CLSCompliant(false)]

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure;

internal sealed class GenericInfrastructureTestServerFixture : IDisposable
{
    public TestServer Server { get; set; }

    public async Task InitializeAsync()
    {
        using var host = new HostBuilder()
            .ConfigureWebHost(webHostBuilder =>
            {
                webHostBuilder
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseEnvironment("IntegrationTest")
                    .UseDefaultServiceProvider(options => { options.ValidateScopes = true; })
                    .ConfigureAppConfiguration((context, builder) => { builder.AddEnvironmentVariables(); })
                    .UseStartup<Startup>();
            })
            .Build();
        await host.StartAsync();
        Server = host.GetTestServer();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Server?.Dispose();
    }
}
