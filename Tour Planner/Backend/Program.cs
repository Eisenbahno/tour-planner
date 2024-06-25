using Backend.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // builder.Services.AddMudServices();
        // //TODO: Add other EF Services per Object
        // builder.Services.AddDbContextFactory<ToursDbContext>((DbContextOptionsBuilder options) 
        //     => options.UseSqlServer(builder.Configuration.GetConnectionString("Default") 
        //                             ?? throw new NullReferenceException("No connection string in config!")));
        
        builder.Services.AddDbContextFactory<ToursDbContext>((DbContextOptionsBuilder options) =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Default")
                              ?? throw new NullReferenceException("No connection string in config!"))
                .EnableSensitiveDataLogging()  // Optional: Aktiviert detaillierte Protokollierung
                .LogTo(Console.WriteLine, LogLevel.Information));


        var app = builder.Build();
        
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        

        app.Run();
    }
}