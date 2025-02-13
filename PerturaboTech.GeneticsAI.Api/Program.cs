using Microsoft.EntityFrameworkCore;
using PerturaboTech.GeneticsAI.Api.Data;
using PerturaboTech.GeneticsAI.Api.Helpers;
using PerturaboTech.GeneticsAI.Api.Helpers.Extensions;
using PerturaboTech.GeneticsAI.Api.Helpers.Options;

namespace PerturaboTech.GeneticsAI.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            var configuration = builder.Configuration;
            // Add services to the container.

            builder.Services.AddControllers();
            services.AddSingleton<DapperContext>();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            Ensure.NotNullOrEmpty(connectionString);
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
            services.AddOptions<DatabaseOptions>()
                .Bind(configuration.GetSection(DatabaseOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                await app.Services.InitializeDbAsync();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            await app.RunAsync();
        }
    }
}
