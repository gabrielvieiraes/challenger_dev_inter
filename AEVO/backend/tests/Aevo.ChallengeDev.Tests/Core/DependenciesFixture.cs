using Aevo.ChallengeDev.Tests.Core;
using DotNet.Testcontainers.Containers;
using Testcontainers.MsSql;

[assembly: AssemblyFixture(typeof(DependenciesFixture))]

namespace Aevo.ChallengeDev.Tests.Core;

public class DependenciesFixture : IAsyncLifetime
{
    private const string ConnectionString = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=True;";

    public DbContainerConnectionInfo GetDbConnectionInfo()
    {
        return new DbContainerConnectionInfo(ConnectionString, null, null);
    }

    public async ValueTask InitializeAsync()
    {
        await Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await Task.CompletedTask;
        GC.SuppressFinalize(this);
    }
}

public record DbContainerConnectionInfo(string dataSource, string user, string password);
