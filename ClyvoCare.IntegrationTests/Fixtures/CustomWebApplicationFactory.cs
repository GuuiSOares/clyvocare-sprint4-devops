using ClyvoCare.API.Data;
using ClyvoCare.API.Data.Repositories;
using ClyvoCare.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ClyvoCare.IntegrationTests.Fixtures;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "ClyvoCareSecretKeyMuitoSeguraParaSprint4DotNet2026",
                ["Jwt:Issuer"] = "ClyvoCareAPI",
                ["Jwt:Audience"] = "ClyvoCareClient",
                ["ConnectionStrings:MongoDbConnection"] = "mongodb://localhost:27017"
            });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
            services.RemoveAll(typeof(DbContextOptions));
            services.RemoveAll(typeof(AppDbContext));
            services.RemoveAll(typeof(ITelemetriaMongoRepository));

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase($"ClyvoCareIntegrationDb_{Guid.NewGuid()}");
            });

            services.AddSingleton<ITelemetriaMongoRepository, TelemetriaMongoRepositoryFake>();

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        });
    }

    private sealed class TelemetriaMongoRepositoryFake : ITelemetriaMongoRepository
    {
        private readonly List<PetTelemetriaNoSql> _itens = new();

        public Task InserirTelemetriaAsync(PetTelemetriaNoSql telemetria)
        {
            _itens.Add(telemetria);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<PetTelemetriaNoSql>> ObterPorPetIdAsync(int petId)
        {
            return Task.FromResult(_itens.Where(x => x.PetId == petId).AsEnumerable());
        }
    }
}
