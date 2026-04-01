using APIwebbanhangnhe.Data;
using APIwebbanhangnhe.Repositories;
using APIwebbanhangnhe.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIwebbanhangnhe;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IPlayerCardRepository, PlayerCardRepository>();
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();

        await DbInitializer.InitializeAsync(app.Services);

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        await app.RunAsync();
    }
}
