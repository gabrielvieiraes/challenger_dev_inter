using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Aevo.ChallengeDev.WebApi.Core.Auth;
using Aevo.ChallengeDev.WebApi.Modulos.Usuarios.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;

namespace Aevo.ChallengeDev.Tests.Core;

public abstract class TestBase(IntegrationTestFactory factory) : IAsyncLifetime, IClassFixture<IntegrationTestFactory>
{
    protected readonly IntegrationTestFactory Factory = factory;
    protected readonly HttpClient Http = factory.CreateClient();
    protected FakeTimeProvider FakeTimeProvider = factory.Services.GetRequiredService<FakeTimeProvider>();

    protected virtual async Task Seed()
    {
        try
        {
            var context = Factory.GetDbContext();

            var usersMock = UsuariosDeTestePredefinidos.ObterTodosOsUsuarios();
            var salasMock = SalasDeTestePredefinidas.ObterTodasAsSalas();

            context.Usuarios.AddRange(usersMock);
            context.Salas.AddRange(salasMock);

            await context.SaveChangesAsync();
        }
        catch (Exception)
        {

        }
    }

    protected void LoginAs(UsuarioDeTeste usuario)
    {
        var authService = Factory.Services.GetRequiredService<IAuthService>();
        var token = authService.GenerateAccessToken(usuario.Id);

        Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async ValueTask InitializeAsync()
    {
        Http.DefaultRequestHeaders.Authorization = null;
        await Factory.ResetDatabaseAsync();

        await Seed();
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}