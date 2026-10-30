using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using ClyvoCare.API.DTOs;
using ClyvoCare.API.Models;
using ClyvoCare.IntegrationTests.Fixtures;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace ClyvoCare.IntegrationTests.Controllers;

public class EndpointsIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public EndpointsIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        AutenticarCliente();
    }

    private void AutenticarCliente()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes("ClyvoCareSecretKeyMuitoSeguraParaSprint4DotNet2026");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "Tutor Integracao"),
                new Claim(ClaimTypes.Email, "integracao@teste.com")
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = "ClyvoCareAPI",
            Audience = "ClyvoCareClient",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", tokenHandler.WriteToken(token));
    }

    [Fact]
    public async Task PostUsuario_PayloadValido_RetornaCreated()
    {
        var usuarioDto = new UsuarioCreateDTO
        {
            Nome = "Tutor Integracao",
            Email = $"tutor_{Guid.NewGuid()}@teste.com",
            Senha = "senhaForte123"
        };

        var response = await _client.PostAsJsonAsync("/api/Usuarios", usuarioDto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task PostPet_ComTokenValido_RetornaCreated()
    {
        var usuarioDto = new UsuarioCreateDTO
        {
            Nome = "Tutor Pet",
            Email = $"tutor_pet_{Guid.NewGuid()}@teste.com",
            Senha = "senhaForte123"
        };

        var usuarioResponse = await _client.PostAsJsonAsync("/api/Usuarios", usuarioDto);
        Assert.Equal(HttpStatusCode.Created, usuarioResponse.StatusCode);
        var usuario = await usuarioResponse.Content.ReadFromJsonAsync<Usuario>();
        Assert.NotNull(usuario);

        var petDto = new PetCreateDTO
        {
            Nome = "Pipoca",
            Especie = "Felino",
            DataNascimento = DateTime.Now.AddYears(-1),
            UsuarioId = usuario!.Id
        };

        var response = await _client.PostAsJsonAsync("/api/Pets", petDto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task PostLogSaude_PayloadValido_RetornaCreated()
    {
        var usuarioDto = new UsuarioCreateDTO
        {
            Nome = "Tutor Log",
            Email = $"tutor_log_{Guid.NewGuid()}@teste.com",
            Senha = "senhaForte123"
        };

        var usuarioResponse = await _client.PostAsJsonAsync("/api/Usuarios", usuarioDto);
        var usuario = await usuarioResponse.Content.ReadFromJsonAsync<Usuario>();
        Assert.NotNull(usuario);

        var petDto = new PetCreateDTO
        {
            Nome = "Thor",
            Especie = "Cachorro",
            DataNascimento = DateTime.Now.AddYears(-2),
            UsuarioId = usuario!.Id
        };

        var petResponse = await _client.PostAsJsonAsync("/api/Pets", petDto);
        Assert.Equal(HttpStatusCode.Created, petResponse.StatusCode);

        var hateoas = await petResponse.Content.ReadFromJsonAsync<HateoasResponse<Pet>>();
        Assert.NotNull(hateoas?.Data);

        var logDto = new LogSaudeCreateDTO
        {
            PetId = hateoas!.Data.Id,
            Peso = 10.5m,
            Temperatura = 38.8m,
            BatimentosCardiacos = 115,
            Observacoes = "Teste de integracao IoT"
        };

        var response = await _client.PostAsJsonAsync("/api/LogsSaude", logDto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
