using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api;
using GtMotive.Estimate.Microservice.Domain.Base;
using GtMotive.Estimate.Microservice.Infrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Testcontainers.MongoDb;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure;

/// <summary>
/// Composition root fixture for functional tests using Testcontainers.
/// This fixture sets up a containerized MongoDB instance for each test collection,
/// ensuring test isolation and reproducibility without requiring a real database.
/// </summary>
/// <remarks>
/// Note: Requires Testcontainers.MongoDb NuGet package and Docker to be installed and running.
/// Install with: dotnet add package Testcontainers.MongoDb
/// </remarks>
[SuppressMessage("Design", "CA1515:Consider making public types internal")] ////TODO: Review
public sealed class CompositionRootTestFixtureWithTestcontainers : IDisposable, IAsyncLifetime
{
    private ServiceProvider _serviceProvider;
    private MongoDbContainer _mongoDbContainer;
    private string mongoConnectionString = string.Empty;
    private const string BbddName = "estimate-test-db";

    public CompositionRootTestFixtureWithTestcontainers()
    {
        // Container will be initialized in InitializeAsync
        _serviceProvider = null!;
        Configuration = null!;
    }

    public IConfiguration Configuration { get; private set; }

    public async Task SeedDatabaseAsync<T>(IEnumerable<T> data, string collectionName, bool clear = false) where T : IDocument
    {
        using var client = new MongoClient(mongoConnectionString);
        var database = client.GetDatabase(BbddName);
        var collection = database.GetCollection<T>(collectionName);

        await collection.DeleteManyAsync(_ => true); // clean
        if (!clear)
        {
            await collection.InsertManyAsync(data);
        }
    }

    /// <summary>
    /// Starts the MongoDB container and initializes the DI container.
    /// This method must be called via xUnit's IAsyncLifetime before tests run.
    /// </summary>
    public async Task InitializeAsync()
    {
        try
        {
            _mongoDbContainer = new MongoDbBuilder(image: "mongo:latest").WithCleanUp(true).Build();

            // Start the MongoDB container
            await _mongoDbContainer.StartAsync();

            // Get the connection string from the running container
            mongoConnectionString = _mongoDbContainer.GetConnectionString();

            var configBuilder = new ConfigurationBuilder();

            // Add IN-MEMORY config FIRST - these take precedence
            configBuilder
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["MongoDb:ConnectionString"] = mongoConnectionString,
                    ["MongoDb:MongoDbDatabaseName"] = BbddName
                });

            Configuration = configBuilder.Build();
            // Verify we're using the container connection string
            var usedConnectionString = Configuration["MongoDb:ConnectionString"];
            if (!usedConnectionString.Contains("mongodb://", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"Configuration is not using container connection string. Got: {usedConnectionString}");
            }

            // Build service collection
            var services = new ServiceCollection();
            services.AddSingleton(Configuration);
            ConfigureServices(services, Configuration);

            // Build service provider
            _serviceProvider = services.BuildServiceProvider();

            // DO NOT initialize MongoDB - tests will seed their own data
            // This ensures each test gets a clean database

        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "Failed to initialize Testcontainers fixture. " +
                "Ensure Testcontainers.MongoDb package is installed and Docker is running. " +
                "Install with: dotnet add package Testcontainers.MongoDb",
                ex);
        }
    }


    /// <summary>
    /// Stops the MongoDB container and disposes resources.
    /// This method is called automatically by xUnit's IAsyncLifetime after tests complete.
    /// </summary>
    public async Task DisposeAsync()
    {
        try
        {
            if (_mongoDbContainer != null)
            {
                await _mongoDbContainer.StopAsync();
                await _mongoDbContainer.DisposeAsync();
                _mongoDbContainer = null;
            }
        }
        catch (ObjectDisposedException)
        {
            // Container already disposed
        }
        catch (InvalidOperationException)
        {
            // Docker not available or container already removed
        }
        catch (TimeoutException)
        {
            // Container shutdown timeout
        }
    }

    public async Task UsingHandlerForRequest<TRequest>(Func<IRequestHandler<TRequest, Unit>, Task> handlerAction)
        where TRequest : IRequest<Unit>
    {
        ArgumentNullException.ThrowIfNull(handlerAction);

        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<TRequest, Unit>>();

        await handlerAction.Invoke(handler);
    }

    public async Task UsingHandlerForRequestResponse<TRequest, TResponse>(Func<IRequestHandler<TRequest, TResponse>, Task> handlerAction)
        where TRequest : IRequest<TResponse>
    {
        ArgumentNullException.ThrowIfNull(handlerAction);

        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>() ?? throw new InvalidOperationException("The requested handler has not been registered");
        await handlerAction.Invoke(handler);
    }

    public async Task UsingRepository<TRepository>(Func<TRepository, Task> handlerAction)
    {
        ArgumentNullException.ThrowIfNull(handlerAction);

        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<TRepository>() ?? throw new InvalidOperationException("The requested repository has not been registered");
        await handlerAction.Invoke(handler);
    }

    public async Task UsingMediator(Func<IMediator, Task> mediatorAction)
    {
        ArgumentNullException.ThrowIfNull(mediatorAction);

        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>() ?? throw new InvalidOperationException("The mediator has not been registered");
        await mediatorAction.Invoke(mediator);
    }

    public void Dispose()
    {
        _serviceProvider?.Dispose();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiDependencies();
        services.AddLogging();
        services.AddBaseInfrastructure(true, configuration);
    }
}
