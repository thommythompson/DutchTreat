using DutchTreat.Services;
using DutchTreat.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

        ConfigurationManager _config = builder.Configuration;

        // Add services to the container.
        builder.Services.AddControllersWithViews()
            .AddNewtonsoftJson(cfg => cfg.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        builder.Services.AddIdentity<StoreUser, IdentityRole>(cfg => 
        {
            cfg.User.RequireUniqueEmail = true;
            cfg.Password.RequiredLength = 8;
            cfg.SignIn.RequireConfirmedAccount = false;
        })
            .AddEntityFrameworkStores<DutchContext>();

        builder.Services.AddAuthentication()
            .AddCookie()
            .AddJwtBearer(cfg => 
            {
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = _config["Tokens:Issuer"],
                    ValidAudience = _config["Tokens:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
                };
            });

        builder.Services.AddDbContext<DutchContext>();

        builder.Services.AddTransient<IMailService, NullMailService>();

        builder.Services.AddTransient<DutchSeeder>();

        builder.Services.AddScoped<IDutchRepository, DutchRepository>();

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

        var app = builder.Build();

        // Seeding
        static void RunSeeding(WebApplication app)
        {
            var scopeFactory = app.Services.GetService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<DutchSeeder>();

                seeder.SeedAsync().Wait();
            }
        }

        // Seed using dotnet run /seed
        if(args.Length > 0 && args[0] == "/seed")
        {
            RunSeeding(app);
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment() && !app.Environment.IsStaging())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action}/{id?}"
            );

        app.Run();
    }
}