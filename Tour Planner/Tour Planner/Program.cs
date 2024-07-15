using Tour_Planner.Components;
using MudBlazor.Services;
using Shared.Service;


namespace Tour_Planner;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddMudServices();
        
        builder.Services.AddSingleton<HttpClient>(sp => 
            new HttpClient { BaseAddress = new Uri("https://api.openrouteservice.org/") });
        
        builder.Services.AddSingleton<OpenRouteService>(sp =>
        {
            var httpClient = sp.GetRequiredService<HttpClient>();
            var configuration = sp.GetRequiredService<IConfiguration>();
            var apiKey = configuration["OpenRouteServiceApiKey"];
            return new OpenRouteService(httpClient, configuration["OpenRouteServiceApiKey"]);
        });
        
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

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}