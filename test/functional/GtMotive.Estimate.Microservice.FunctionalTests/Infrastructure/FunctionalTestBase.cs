using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure;

[SuppressMessage("Design", "CA1515:Consider making public types internal")] ////TOO: Review
[Collection(TestCollections.Functional)]
public abstract class FunctionalTestBase(CompositionRootTestFixture fixture) : IAsyncLifetime
{
    public const int QueueWaitingTimeInMilliseconds = 1000;

    protected CompositionRootTestFixture Fixture { get; } = fixture;

    public async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await Task.CompletedTask;
    }
}
