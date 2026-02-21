using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

[assembly: CLSCompliant(false)]

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure;

#pragma warning disable CA1515 // Considere la posibilidad de hacer que los tipos públicos sean internos
public sealed class GenericInfrastructureTestServerFixture : IDisposable
#pragma warning restore CA1515 // Considere la posibilidad de hacer que los tipos públicos sean internos
{
    public TestServer Server { get; set; }

    public async Task InitializeAsync()
    {
        var host = new HostBuilder()
            .ConfigureWebHost(webHostBuilder =>
            {
                webHostBuilder
                    .UseTestServer()
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
