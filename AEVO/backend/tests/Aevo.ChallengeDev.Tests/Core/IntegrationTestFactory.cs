using System.Runtime.CompilerServices;
using Aevo.ChallengeDev.WebApi;
using Aevo.ChallengeDev.WebApi.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Time.Testing;
using Respawn;

namespace Aevo.ChallengeDev.Tests.Core;

public class IntegrationTestFactory : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
{
    private readonly Guid _testScopeId = Guid.NewGuid();
    private Respawner _respawner = default!;
    private readonly string _dbConnectionString;

    public static DateOnly DefaultDate => new(2024, 08, 24);

    public static DateTimeOffset DefaultDateTime =>
        new(DefaultDate, TimeOnly.FromTimeSpan(TimeSpan.Zero), TimeSpan.Zero);

    public IntegrationTestFactory(DependenciesFixture dependenciesFixture)
    {
        _dbConnectionString = BuildDbConnString(dependenciesFixture.GetDbConnectionInfo());
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        Environment.SetEnvironmentVariable("Auth__AccessTokenKey", "my-very-secure-access-token-key-🔒🔒🔒🔒🔒🔒");
        Environment.SetEnvironmentVariable("Auth__AccessTokenHoursLifetime", "12");

        builder.ConfigureServices(services =>
        {
            var serviceToRemove = new List<Type>()
            {
                typeof(DbContextOptions<Context>),
                typeof(Context),
                typeof(IHostedService),
            };
            var contextsDescriptor = services.Where(d => serviceToRemove.Contains(d.ServiceType)).ToArray();
            foreach (var descriptor in contextsDescriptor)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<Context>(options =>
            {
                options.UseSqlServer(_dbConnectionString)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            });

            services.AddSingleton<FakeTimeProvider>(_ => new FakeTimeProvider(DefaultDateTime));
            services.AddSingleton<TimeProvider>(sp => sp.GetRequiredService<FakeTimeProvider>());
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string BuildDbConnString(DbContainerConnectionInfo dbConnInfo) =>
        new SqlConnectionStringBuilder
        {
            DataSource = @"(localdb)\MSSQLLocalDB",
            IntegratedSecurity = true,
            InitialCatalog = $"testAEVOdb",
            TrustServerCertificate = true
        }.ConnectionString;

    public Context GetDbContext()
    {
        return Services.GetRequiredService<Context>();
    }

    public async Task ResetDatabaseAsync()
    {
        await using var conn = new SqlConnection(_dbConnectionString);
        await conn.OpenAsync();
        await _respawner.ResetAsync(conn);
    }

    public async ValueTask InitializeAsync()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<Context>();
        dbContextOptionsBuilder.UseSqlServer(_dbConnectionString)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();

        var context = new Context(dbContextOptionsBuilder.Options);
        await context.Database.EnsureCreatedAsync();

        await SetupRespawnerAsync(_dbConnectionString);
    }

    private async Task SetupRespawnerAsync(string connString)
    {
        _respawner = await Respawner.CreateAsync(connString,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.SqlServer,
                SchemasToInclude = ["dbo"],
                WithReseed = true
            });
    }
}
