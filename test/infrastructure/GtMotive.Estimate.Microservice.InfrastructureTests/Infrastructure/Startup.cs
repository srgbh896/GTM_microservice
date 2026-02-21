using System.Collections.Generic;
using System.IO;
using System.Threading;
using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;
using GtMotive.Estimate.Microservice.Api;
using GtMotive.Estimate.Microservice.Api.Features.Vehicles.GetAllVehicles;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure;

internal sealed class Startup(IWebHostEnvironment environment, IConfiguration configuration)
{
    public IWebHostEnvironment Environment { get; } = environment;

    public IConfiguration Configuration { get; } = configuration;

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public static void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.Use(async (context, next) =>
        {
            context.Response.Body = new MemoryStream();
            await next();
        });
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public static void ConfigureServices(IServiceCollection services)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["MongoDb:ConnectionString"] = "TestValue",
                ["AnotherSetting"] = "123"
            })
            .AddEnvironmentVariables() // optional
            .Build();
        var presenterMock = new Mock<IWebApiPresenter>();
        presenterMock
            .Setup(p => p.ActionResult)
            .Returns(new OkResult());
        services.AddRouting();
        services.AddControllers();
        services.AddSingleton(_ =>
        {
            var mock = new Mock<IMediator>();
            mock.Setup(m => m.Send(It.IsAny<CreateVehicleCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(presenterMock.Object);

            return mock.Object;
        });

        services.AddAuthentication(TestServerDefaults.AuthenticationScheme)
            .AddTestServer();

        services.AddControllers(ApiConfiguration.ConfigureControllers)
            .WithApiControllers();

        services.AddBaseInfrastructure(true, config);
    }
}
