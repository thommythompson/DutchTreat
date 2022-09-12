using DutchTreat.Services;
using DutchTreat.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DutchTreat;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Import configuration
        builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<DutchContext>();

        builder.Services.AddTransient<IMailService, NullMailService>();

        builder.Services.AddTransient<DutchSeeder>();

        builder.Services.AddScoped<IDutchRepository, DutchRepository>();

        var app = builder.Build();

        // Seeding
        static void RunSeeding(WebApplication app)
        {
            var scopeFactory = app.Services.GetService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<DutchSeeder>();

                seeder.Seed();
            }
        }

        RunSeeding(app);

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment() && !app.Environment.IsStaging())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action}/{id?}"
            );

        app.Run();
    }
}
