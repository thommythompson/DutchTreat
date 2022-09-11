using DutchTreat.Services;
using DutchTreat.Data;
using Microsoft.EntityFrameworkCore;

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

        var app = builder.Build();

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
