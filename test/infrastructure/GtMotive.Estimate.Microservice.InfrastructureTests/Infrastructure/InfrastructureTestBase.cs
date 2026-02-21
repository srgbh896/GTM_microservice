using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure;

[Collection(TestCollections.TestServer)]
#pragma warning disable CA1515 // Considere la posibilidad de hacer que los tipos públicos sean internos
public abstract class InfrastructureTestBase(GenericInfrastructureTestServerFixture fixture)
#pragma warning restore CA1515 // Considere la posibilidad de hacer que los tipos públicos sean internos
{
    protected GenericInfrastructureTestServerFixture Fixture { get; } = fixture;
}
