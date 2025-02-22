using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Aevo.ChallengeDev.Tests.Core;
using Aevo.ChallengeDev.WebApi.Modulos.Usuarios.Endpoints;
using Newtonsoft.Json;

namespace Aevo.ChallengeDev.Tests.Modulos
{
    public class UsuariosTests(IntegrationTestFactory factory) : TestBase(factory)
    {
        [Fact]
        public async Task Login_DevePermitirLogin()
        {
            //// Arrange
            var req = new Login
            {
                Email = "joao.silva@example.com",
                Idioma = "pt-BR",
                Password = "SenhaSegura123"
            };

            //// Act
            var response = await Http.PostAsJsonAsync($"usuarios/login", req, cancellationToken: TestContext.Current.CancellationToken);

            //// Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
        }

        [Fact]
        public async Task Login_DeveFalharLogin()
        {
            //// Arrange
            var req = new Login
            {
                Email = "joao.silva@example.com",
                Idioma = "pt-BR",
                Password = "SenhaSegura1234"
            };

            //// Act
            var response = await Http.PostAsJsonAsync($"usuarios/login", req, cancellationToken: TestContext.Current.CancellationToken);

            //// Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Register_DeveCriarUsuario()
        {
            // Arrange
            var req = new RegistrarUsuario
            {
                Nome = "Gabriel",
                Email = $"gabriel.{Guid.NewGuid().ToString()}@example.com",
                Idioma = "Português",
                FusoHorario = "UTC-3",
                Password = "Senha@123"
            };

            //// Act
            var response = await Http.PostAsJsonAsync($"usuarios", req, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
        }

        [Fact]
        public async Task Register_DeveFalharPorJaExistirUsuarioComMesmoEmail()
        {
            // Arrange
            var req = new RegistrarUsuario
            {
                Nome = "João",
                Email = $"joao.silva@example.com",
                Idioma = "Português",
                FusoHorario = "UTC-3",
                Password = "Senha@123"
            };

            //// Act
            var response = await Http.PostAsJsonAsync($"usuarios", req, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(response.IsSuccessStatusCode);            
        }
    }
}
